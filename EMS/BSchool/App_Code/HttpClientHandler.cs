using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;

public class HttpClientHandler
{
}
public class MyProxy : IWebProxy
{
    string localUsername = Convert.ToString(ConfigurationManager.AppSettings["localUsername"]);
    string localPassword = Convert.ToString(ConfigurationManager.AppSettings["localPassword"]);
    string localDomain = Convert.ToString(ConfigurationManager.AppSettings["localDomain"]);
    public ICredentials Credentials
    {
        get { return new NetworkCredential(localUsername, localPassword, localDomain); }        
        set { }
    }

    public Uri GetProxy(Uri destination)
    {
        return new Uri("http://192.168.1.1:8080");
    }

    public bool IsBypassed(Uri host)
    {
        return false;
    }
}