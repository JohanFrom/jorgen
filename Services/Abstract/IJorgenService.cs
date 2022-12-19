namespace jorgen.Services.Abstract
{
    public interface IJorgenService
    {
        public string GetBeardStatus(double temp);
        public byte[] GetJorgenImage();
    }
}
