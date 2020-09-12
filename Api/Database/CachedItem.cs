using System;

namespace Api.Database
{
    public class CachedItem
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}