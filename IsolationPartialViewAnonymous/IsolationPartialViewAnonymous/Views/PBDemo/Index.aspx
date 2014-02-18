<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>

    <p><%= ViewData["ViewType"] %></p>

    <form action="<% Url.RouteUrl("Default", new { controller = "PBDemo", action = "Index" }); %>" method="post">
        <input type="submit" value="Submit" />
    </form>

</asp:Content>
