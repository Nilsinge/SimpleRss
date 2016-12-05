using System;
using Logic.Entities;

namespace Logic.Service.Validation
{
    public class FeedValidator
    {
        private readonly UrlValidator urlValidator = new UrlValidator();

        public bool Validate(string urlValue, FeedCategory category)
        {
            throw new NotImplementedException();
        }
    }
}