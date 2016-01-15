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

    public class MessageRepository:DeletableEntityRepository<Message>
    {
        public MessageRepository(IDbContext context)
            : base(context)
        {

        }

        public delegate void OnMessageUpdateDelegate(Message message);

        public OnMessageUpdateDelegate OnMessageUpdate;

        public OnMessageUpdateDelegate OnMessageDelete;

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

        public override void Delete(object id)
        {
            this.OnMessageDelete(this.GetById(id));
            base.Delete(id);
        }

        public override void Delete(Message entity)
        {
            this.OnMessageDelete(entity);
            base.Delete(entity);
        }
    }
}
