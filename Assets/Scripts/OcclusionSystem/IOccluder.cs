
namespace MS.OpenWorld.Occluder
{
    public interface IOccluder
    {
        public void OnLoadGame();
        public void OnOccluding();
        public void OnDeoccluding();
    }
}
