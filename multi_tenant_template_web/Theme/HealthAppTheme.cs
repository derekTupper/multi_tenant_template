using MudBlazor;

namespace CMS.Web.Theme
{
    public class HealthAppTheme : MudTheme
    {
        public HealthAppTheme()
        {
            Palette = new Palette()
            {
                Primary = Colors.Blue.Darken1,
                Secondary = Colors.DeepPurple.Accent2,
                Background = Colors.Grey.Lighten5,
                AppbarBackground = Colors.Grey.Lighten5,
                DrawerBackground = "#FFF",
                DrawerText = "rgba(0,0,0, 0.7)",
                AppbarText = "rgba(0,0,0, 0.7)",
                Success = "#06d79c",
                TextDisabled = "rgba(0,0,0, 0.7)"
            };

            LayoutProperties = new LayoutProperties()
            {
                DefaultBorderRadius = "3px"
            };

            Shadows = new Shadow();
            ZIndex = new ZIndex();
        }
    }
}
