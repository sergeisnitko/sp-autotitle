﻿using SP.Cmd.Deploy;

namespace SPF.AutoTitle
{
    class Program
    {
        static void Main(string[] args)
        {


            args = "--url https://snitko.sharepoint.com/sites/dev01/AutoTitle01 --execute --login sergei@snitko.onmicrosoft.com --password +Martezacker) --spo".Split(' ');
            //args = "--url http://republic.arvosys.com/test/Communications --deploy --login sergei.snitko --domain republic --password +Martezacker)".Split(' ');
            //args = "--url http://demo.arvosys.com/sites/city02/documents --execute --login spsql --domain cib --password 1qazxsw@".Split(' ');

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
