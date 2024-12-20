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
        public void ShowSnackBar(string message, bool isSuccess, string position = "bottom-center")
        {
            if (isSuccess)
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

                else
                {
                    Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomCenter;

                }

                //then show the snack bar
                Snackbar.Add(message, Severity.Success);
            }

            //if the outcome was a failure
            else
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;

                Snackbar.Add(message, Severity.Error);

            }



        }

    }
}
