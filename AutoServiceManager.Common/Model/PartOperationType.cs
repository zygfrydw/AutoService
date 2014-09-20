using AutoServiceManager.Common.DataAnnotations;
using AutoServiceManager.Website.Models;
using System.ComponentModel;

namespace AutoServiceManager.Common.Model
{
    public enum PartOperationType
    {
        [Description("Zgłoszenie zapotrzebowanie")]
        [Color("red")]
        [HtmlClass("warning")]
        Request,
        [Description("Złożone zamówienie")]
        [Color("red")]
        [HtmlClass("info")]
        Approved,
        [Description("Odrzucone zapotrzebowanie")]
        [Color("red")]
        [HtmlClass("danger")]
        Rejected,
        [Description("Części wydane")]
        [Color("green")]
        [HtmlClass("success")]
        Released
    }
}