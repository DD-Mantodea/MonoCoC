using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes
{
    public class PagedMenu<T>
    {
        public const int DefaultPageSize = 12;

        public const int MaxPageSize = 12;

        public IReadOnlyList<T> Items = [];

        public int PageSize = DefaultPageSize;

        public int Page { get; set; }

        public int PageCount => Math.Max(1, (Items.Count + PageSize - 1) / PageSize);

        public void Reset() => Page = 0;

        public void Render(
            Action<int, string, Action?> addButton,

            Func<T, string> label,

            Func<T, Action?> action,

            Action back,

            Action refresh,

            string previous,

            string next,

            string backLabel,

            int startSlot = 0)
        {
            PageSize = Math.Clamp(PageSize, 1, MaxPageSize - startSlot);

            Page = Math.Clamp(Page, 0, PageCount - 1);

            var start = Page * PageSize;

            var end = Math.Min(start + PageSize, Items.Count);

            for (var index = start; index < end; index++)
                addButton(startSlot + index - start, label(Items[index]), action(Items[index]));

            addButton(startSlot + PageSize, previous, Page > 0

                ? () =>
                {
                    Page--;

                    refresh();
                }

                : null);

            addButton(startSlot + PageSize + 1, next, Page < PageCount - 1

                ? () =>
                {
                    Page++;

                    refresh();
                }

                : null);

            addButton(14, backLabel, back);
        }
    }
}