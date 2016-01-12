/// <summary>
/// Summary description for MessageRepository
/// </summary>
namespace CollectingProductionDataSystem.Data.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.UtilityEntities;

    public delegate void OnMessageUpdateDelegate(Message message);

    public class MessageRepository:DeletableEntityRepository<Message>
    {
        public MessageRepository(IDbContext context)
            : base(context)
        {
            this.OnMessageUpdate = new OnMessageUpdateDelegate(MessageUpdate);
        }

        public OnMessageUpdateDelegate OnMessageUpdate { get; set; }

        public override void Add(Message entity)
        {
            this.OnMessageUpdate(entity);
            base.Add(entity);
        }

        public override void Update(Message entity)
        {
            this.OnMessageUpdate(entity);
            base.Update(entity);
        }

        private void MessageUpdate(Message message){}
    }
}
