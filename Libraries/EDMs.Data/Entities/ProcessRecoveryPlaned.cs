
namespace EDMs.Data.Entities
{
    public partial class ProcessRecoveryPlaned
    {
        public string RevcoveryName
        {
            get { return "Recovery Plan - " + this.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy"); }
        }
    }
}
