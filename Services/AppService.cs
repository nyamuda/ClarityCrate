using MudBlazor;


namespace Clarity_Crate.Services
{
    public class AppService
    {

        public ISnackbar Snackbar;
       


        public AppService(ISnackbar snackbar)
        {
            Snackbar = snackbar;
        }



        //show a snack bar
        public void ShowSnackBar(string message, string position = "bottom-center",string severity ="normal")
        {
            //first set the position of the snack bar
            if (position.Equals("top-right"))
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;

            }
            else if (position.Equals("bottom-right"))
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;

            }
            else if (position.Equals("bottom-left"))
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;

            }

            else
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomCenter;

            }

            //then show the severity of the snack bar
            if (severity.Equals("success"))
            {  
                Snackbar.Add(message, Severity.Success);
            }
            else if (severity.Equals("warning"))
            {
                Snackbar.Add(message, Severity.Warning);

            }
            else if (severity.Equals("info"))
            {
                Snackbar.Add(message, Severity.Info);

            }
            else if(severity.Equals("error"))
            {

                Snackbar.Add(message, Severity.Error);
            }

            //default severity is normal
            else
            {

                Snackbar.Add(message, Severity.Normal);

            }



        }

    }
}
