namespace ComputerInfo.Models.Models
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public int PCInfoId { get; set; }
        public virtual PCInfo PCInfo { get; set; }
    }
}
