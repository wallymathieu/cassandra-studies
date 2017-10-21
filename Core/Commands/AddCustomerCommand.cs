using Core.Domain;

namespace Core.Commands
{
    public class AddCustomerCommand : Command
    {
        public virtual int Id { get; private set; }

        public virtual int Version { get; private set; }

        public virtual string Firstname { get; private set; }

        public virtual string Lastname { get; private set; }

        public override void Handle(IRepository repository)
        {
            var command = this;
            repository.Save(new Customer(command.Id, command.Firstname, command.Lastname, command.Version));
        }
    }
}
