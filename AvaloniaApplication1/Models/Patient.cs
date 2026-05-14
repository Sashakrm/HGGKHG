using System;
using AvaloniaApplication1.Security;

namespace AvaloniaApplication1.Models;

public sealed class Patient //: Secure_Storage
{
    public int id { get; }
    public string full_name { get; }

    private string _encrypted_contacts;
    private string _encrypted_birth_date;

    public string Contacts
    {
        get
        {
            if(string.IsNullOrEmpty(_encrypted_contacts))
                return string.Empty;
            
            //return Crypto.
            return _encrypted_contacts;
        }
    }

    public Patient(){}
}