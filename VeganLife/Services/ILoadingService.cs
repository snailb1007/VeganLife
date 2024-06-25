namespace VeganLife.Services
{
    public interface ILoadingService
    {
        Task<IDisposable> Show(ushort delayTime = 0);
    }
}