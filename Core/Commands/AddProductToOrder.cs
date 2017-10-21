using Core.Domain;
using With;
using With.ReadonlyEnumerable;

namespace Core.Commands
{
    public class AddProductToOrder : Command
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public override void Handle(IRepository repository)
        {
            var command = this;
            var order = repository.GetOrder(command.OrderId);
            repository.Save(order.With(o =>
                o.Products.Add(command.ProductId)));
        }
    }
}
