<%@ Page Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="UO_WebApplication.Default" Title="UO MES WebApplication" %>
<asp:Content ID="Head1" ContentPlaceHolderID="Head1" runat="server">
<% if (false) { %>
<script type="text/javascript" src="src/jquery-1.3.2-vsdoc.js"></script>
<% } %>
</asp:Content>
<asp:Content ID="Holder1" ContentPlaceHolderID="Holder1" runat="server">
    <h1>Page title</h1>
    <img src="/images/photo-small.jpg" class="photosmall" width="150" height="100" alt="Write a short description of the image here. It will show if the image is not loaded. Non visual browsers and search engines will also read this text." title="Users will see this text when they roll over this image. Non-visual browsers will read this text to blind users." />
    <p>Most of the text on this page &quot;Greeked&quot;. Its fake text used to approximate how your content will look. This page has many sample elements (a form, a table, lists, etc..). Use these elements to build out your site. Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Donec molestie. Sed aliquam sem ut arcu. Del sam familie. Lor separat existentie es un myth. Por scientie, musica, sport etc., li tot Europa usa li sam vocabularium.Praesent aliquet pretium erat. Praesent non odio. Pellentesque a magna a mauris vulputate lacinia. Aenean viverra per conubia nostra, per. <a href="#">read more</a> </p>
    <table class="table" border="1" cellspacing="0" summary="Summarise the content of the table here. This table summary does not appear on screen, but is read by non-visual browsers and your blind users.">
      <caption>
      Short description of table contents
      </caption>
      <thead>
        <tr>
          <th scope="col" abbr="name">Widget Name</th>
          <th scope="col">Price</th>
          <th scope="col">Features</th>
          <th scope="col" abbr="in stock">Currently in stock</th>
        </tr>
      </thead>
      <tfoot>
        <tr>
          <th scope="col" abbr="name">Widget Name</th>
          <th scope="col">Price</th>
          <th scope="col">Features</th>
          <th scope="col" abbr="in stock">Currently in stock</th>
        </tr>
      </tfoot>
      <tbody>
        <tr class="table-row-1">
          <td>Super widget</td>
          <td>$30.00</td>
          <td>500 hours</td>
          <td>yes</td>
        </tr>
        <tr class="table-row-2">
          <td>Mega widget</td>
          <td>$25.00</td>
          <td>200 hours</td>
          <td>yes</td>
        </tr>
        <tr class="table-row-1">
          <td>Basic widget</td>
          <td>$20.00</td>
          <td>100 hours</td>
          <td>no</td>
        </tr>
        <tr class="table-row-2">
          <td>Plain widget</td>
          <td>$15.00</td>
          <td>50 hours</td>
          <td>yes</td>
        </tr>
        <tr class="table-row-1">
          <td>Widget lite</td>
          <td>free!</td>
          <td>2 hours</td>
          <td>yes</td>
        </tr>
      </tbody>
    </table>
    <div id="three-column-container">
      <div id="three-column-left">
        <h2>Column one of a three column content section</h2>
        Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut. Column one of a three column content section </div>
      <div id="three-column-right">
        <h2>Column three of a three column content section</h2>
        Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. </div>
      <div id="three-column-middle">
        <h2>Column two of a three column content section</h2>
        Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. </div>
      <div class="clear"></div>
    </div>
    <p>Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Donec molestie. Sed aliquam sem ut arcu. Phasellus sollicitudin. Vestibulum condimentum facilisis nulla. In hac habitasse platea dictumst. Nulla nonummy. Cras quis libero. Cras venenatis. Aliquam posuere lobortis pede. Nullam fringilla urna id leo. Praesent aliquet pretium erat. Praesent non odio. Pellentesque a magna a mauris vulputate lacinia. Aenean viverra. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos hymenaeos. Aliquam lacus. <a href="#" title="Users will see this text when they roll over this link. Keep it short and consise. Use this text to clarify the purpose of the link.">read more</a> </p>
    <img src="/images/photo-big.jpg" class="photobig" width="292" height="195" alt="Write a short description of the image here. It will show if the image is not loaded. Non visual browsers and search engines will also read this text." title="Users will see this text when they roll over this image. Non-visual browsers will read this text to blind users." />
    <h2>Section title</h2>
    <p>Li Europan lingues es membres del sam familie. Lor separat existentie es un myth. Por scientie, musica, sport etc., li tot Europa usa li sam vocabularium. Li lingues differe solmen in li gram matica, li pronunciation.</p>
    <h2>Section title </h2>
    <ul class="list">
      <li>list item one</li>
      <li>list item two</li>
      <li>list item three</li>
      <li>list item four </li>
    </ul>
    <h2>Section title </h2>
    <ul class="link-list-vertical">
      <li><a href="#" title="Users will see this text when they roll over this link. Keep it short and consise. Use this text to clarify the purpose of the link.">list link item one</a></li>
      <li><a href="#" title="Users will see this text when they roll over this link. Keep it short and consise. Use this text to clarify the purpose of the link.">list link item two</a></li>
      <li><a href="#" title="Users will see this text when they roll over this link. Keep it short and consise. Use this text to clarify the purpose of the link.">list link item three</a></li>
      <li><a href="#" title="Users will see this text when they roll over this link. Keep it short and consise. Use this text to clarify the purpose of the link.">list link item four</a></li>
    </ul>
</asp:Content>
