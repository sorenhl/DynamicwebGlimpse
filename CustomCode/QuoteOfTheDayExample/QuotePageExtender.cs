using System;
using System;
using System.Collections.Generic;
using Dynamicweb;
using Dynamicweb.Rendering;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;
using Glimpse.Core.Tab;
using NLog;

namespace GlimpseDemo.CustomCode.QuoteOfTheDayExample
{
    public class QuotePageExtender : PageTemplateExtender
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public override void ExtendTemplate(Template template)
        {
            var from = DateTime.Now;
            var quoteService = new QuoteOfTheDayService();
            try
            {
                var quote = quoteService.GetQuote();
                template.SetTag("QuoteOfTheDay", quote);
                _logger.Debug(new {
                    quote = quote,
                    duration = DateTime.Now - from
                });
            }
            catch (Exception e)
            {
                _logger.Error(new
                {
                    exception = e,
                    duration = DateTime.Now - from
                });
            }
            
        }

    }
}