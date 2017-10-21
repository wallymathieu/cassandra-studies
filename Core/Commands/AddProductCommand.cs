using Core.Domain;

namespace Core.Commands
{
    public class AddProductCommand : Command
    {
        public virtual int Id { get; private set; }
        public virtual int Version { get; private set; }
        public virtual float Cost { get; private set; }
        public virtual string Name { get; private set; }

        public override void Handle(IRepository repository)
        {
            var command = this;
            repository.Save(new Product(command.Id, command.Cost, command.Name, command.Version));
        }
    }
}
