﻿namespace Marketplace.Framework
{
    public abstract class Entity<TId> where TId : Value<TId>
    {
        public TId Id { get; set; }
        private readonly List<object> _events;

        protected Entity() => _events = new List<object>();

        protected void Apply(object @event)
        {
            When(@event);
            EnsureValidState();
            _events.Add(@event);
        }

        protected abstract void When(object @event);

        public IEnumerable<object> GetChanges() => _events.AsEnumerable();

        public void ClearChanges() => _events.Clear();

        protected abstract void EnsureValidState();
    }
}
