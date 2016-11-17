using Microsoft.SharePoint.Client.Utilities;
using SP.Cmd.Deploy;
using System.Web;

namespace SPF.AutoTitle
{
    class Program
    {
        static void Main(string[] args)
        {
            SharePoint.CmdExecute(args, "SPF AutoTitle Solution",
                options =>
                {
                    Model.Deploy(options);
                },
                options =>
                {
                    Model.Retract(options);
                },
                options =>
                {
                    Model.Execute(options);
                }
            );

            var Stop = "";
        }
    }
}
