namespace EETLauncherWPF
{
    public class EETLauncherEETGui {

        public string ChangeTo { get; set; }
        public string Current { get; set; }

        public EETLauncherEETGui() {
            Current = "BG2";
            ChangeTo = "SoD";
        }
        public EETLauncherEETGui( string CurrentGui ) {
            Current = CurrentGui;
        }
        public EETLauncherEETGui( string CurrentGui, string ChangeToGui ) {
            Current = CurrentGui;
            ChangeTo = ChangeToGui;
        }
    }
}
