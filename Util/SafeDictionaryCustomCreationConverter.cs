using System;
using System.Collections.Generic;
using CG.Commons.Collections;
using Newtonsoft.Json.Converters;

namespace Stratus.Util
{
    class SafeDictionaryCustomCreationConverter<TKey, TValue> : CustomCreationConverter<IDictionary<TKey, TValue>>
    {
        private readonly TValue _defaultValue;

        public SafeDictionaryCustomCreationConverter(TValue defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public SafeDictionaryCustomCreationConverter()
        {
            _defaultValue = default(TValue);
        }

        public override IDictionary<TKey, TValue> Create(Type objectType)
        {
            return new SafeDictionary<TKey, TValue>(_defaultValue);
        }
    }
}
