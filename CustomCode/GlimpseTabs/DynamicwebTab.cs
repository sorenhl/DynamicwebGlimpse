using System;
using Dynamicweb.Frontend;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Tab.Assist;

namespace GlimpseDemo.CustomCode.GlimpseTabs
{
    public class DynamicwebTab : ITab, ITabLayout
    {
        private static readonly object Layout = TabLayout.Create()
                                                         .Row(r =>
                                                         {
                                                             r.Cell(0);
                                                             r.Cell(1).WidthInPixels(150).Suffix(" ms").AlignRight().Prefix("T+ ").Class("mono");
                                                             r.Cell(2).WidthInPixels(100).Suffix(" ms").AlignRight().Class("mono");
                                                         }).Build();


        public string Name
        {
            get { return "Dynamicweb"; }
        }

        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.EndRequest; }
        }

        public Type RequestContextType
        {
            get { return null; }
        }

        public object GetData(ITabContext context)
        {
            var section = Plugin.Create("Message", "From Request Start", "From Last");

            var pageView = PageView.Current();
            if (pageView != null && pageView.Execution != null)
            {
                DateTime? previousTimestamp = null, 
                    firstTimestamp = null;
                
                foreach (var entry in pageView.Execution.GetExecutionEntries(true))
                {
                    // Ensure timestamps are set
                    firstTimestamp = firstTimestamp ?? entry.Item2;
                    previousTimestamp = previousTimestamp ?? entry.Item2;

                    section.AddRow()
                        .Column(entry.Item1)
                        .Column(entry.Item2 - firstTimestamp)
                        .Column(entry.Item2 - previousTimestamp);

                    previousTimestamp = entry.Item2;
                }
            }
            return section.Build();
        }

        public object GetLayout()
        {
            return Layout;
        }

    }
}