using System;
using Core.Domain;

namespace Core.Commands
{
    public class AddOrderCommand : Command
    {
        public virtual int Id { get; private set; }
        public virtual int Version { get; private set; }
        public virtual int Customer { get; private set; }
        public virtual DateTime OrderDate { get; private set; }

        public override void Handle(IRepository repository)
        {
            var command = this;
            repository.Save(new Order(command.Id, command.Customer, command.OrderDate, new int[0], command.Version));
        }
    }
}
