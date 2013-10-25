<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_AddressControl.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_AddressControl" %>
      
<script language="javascript">

    Aqufit.Page.Controls.atiAddress = function(id) { // New object constructor
        this.id = id;
        this.GMAP = null;
        this.city = '';
        this.coutryCode = '';
        this.region = '';
        this.center = null;
        this.geocoder = null;
       
    };

    Aqufit.Page.Controls.atiAddress.prototype = {      
        gmapLoad: function() {
            var that = this;
            var latlng = new google.maps.LatLng(-34.397, 150.644);
            var myOptions = {
                zoom: 8,
                center: latlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            this.GMAP = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
            this.center = new google.maps.LatLng(39.6395, -95.4492);    // center on USA
            this.GMAP.setCenter(this.center);
            this.GMAP.setZoom(2);
            var hiddenLat = document.getElementById('<%=hiddenLat.ClientID%>');
            var hiddenLng = document.getElementById('<%=hiddenLng.ClientID%>');
            /*
            google.maps.event.addListener(this.GMAP, "move", function() {
                that.center = GMAP.getCenter();
                hiddenLat.value = that.center.lat();
                hiddenLng.value = that.center.lng();
            });
            */
            hiddenLat.value = this.center.lat();
            hiddenLng.value = this.center.lng();

            this.PopulateRegionsFromCountry();
            this.geocoder = new google.maps.Geocoder();
        },
        addAddressToMap: function(results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                this.GMAP.setCenter(results[0].geometry.location);
                this.GMAP.setZoom(10);
                var hiddenLat = document.getElementById('<%=hiddenLat.ClientID%>');
                var hiddenLng = document.getElementById('<%=hiddenLng.ClientID%>');
                this.center = this.GMAP.getCenter()
                hiddenLat.value = this.center.lat();
                hiddenLng.value = this.center.lng();
            } else {
               // alert("Geocode was not successful for the following reason: " + status);
            }
        },
        getFormAddress: function() {
            var atiTxtAddress = document.getElementById('<%=atiTxtAddress.ClientID %>');
            var atiTxtCity = document.getElementById('<%=atiTxtCity.ClientID %>');
            var atiTxtPostal = document.getElementById('<%=atiTxtPostal.ClientID %>');
            var ddlCountry = document.getElementById('<%=ddlCountry.ClientID %>');
            var ddlRegion = document.getElementById('<%=ddlRegion.ClientID %>');
            var country = ddlCountry[ddlCountry.selectedIndex].innerHTML;
            var region = ddlRegion[ddlRegion.selectedIndex].innerHTML;
            if (region == '') {
                var txtRegion = document.getElementById('<%=txtRegion.ClientID%>');
                region = txtRegion.value;
            }
            var address = atiTxtAddress.value + ", " + atiTxtCity.value + ", " + region + ", " + atiTxtPostal.value + ", " + country;
            return address;
        },
        storeLatLngHomeResponse: function(results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                var HiddenHomeLat = document.getElementById('<%=HiddenHomeLat.ClientID %>');
                var HiddenHomeLng = document.getElementById('<%=HiddenHomeLng.ClientID %>');
                HiddenHomeLat.value = results[0].geometry.location.lat();
                HiddenHomeLng.value = results[0].geometry.location.lng();
            } else {
                // TODO: we need to get a defualt lat lng for this and a good error message and a log
                //alert("Geocode was not successful for the following reason: " + status);
            }
        },
        clearDropDown: function(ddl) {
            for (var i = (ddl.options.length - 1); i >= 0; i--) {
                ddl.options[i] = null;
            }
        },
        PopulateRegionsFromCountry: function() {
            var that = this;
            var countryCode = $('#<%=ddlCountry.ClientID%>').find(':selected').val();    
            Affine.WebService.RegisterService.GetRegionsFromCountry(countryCode, function(result){ that.PopulateRegionsFromCountryCallback(result) }, WebServiceFailedCallback);        
        },
        PopulateRegionsFromCountryCallback: function(result) {
            var ddlRegion = document.getElementById('<%=ddlRegion.ClientID%>');
            var txtRegion = document.getElementById('<%=txtRegion.ClientID%>');
            if (result.TextArray.length > 0) {
                this.clearDropDown(ddlRegion);
                txtRegion.style.display = 'none';
                ddlRegion.style.display = 'block';            
                for (var i = 0; i < result.TextArray.length; i++) {
                    ddlRegion.options[i] = new Option(result.TextArray[i], result.ValueArray[i]);
                    if (result.ValueArray[i] == this.region) {
                        ddlRegion.options[i].selected = true;
                    }
                }
            } else {                        
                ddlRegion.style.display = 'none';
                txtRegion.style.display = 'block';
                txtRegion.value = _region;
            }
            var plRegion = document.getElementById('<%=plRegion.ClientID%>_lblLabel');
            plRegion.innerHTML = result.RegionText;
            var plPostal = document.getElementById('<%=plPostal.ClientID%>_lblLabel');
            plPostal.innerHTML = result.PostalText;
        },
        storeLatLngHome: function() {
            var that = this;
            this.geocoder.geocode({ 'address': this.getFormAddress() }, function(res, stat){ that.storeLatLngHomeResponse(res,stat); });
            return false;   
        },
        centerToAddress: function() {
            var that = this;
            this.geocoder.geocode({ 'address': this.getFormAddress() } , function(res,stat){ that.addAddressToMap(res,stat); });
            return false;       
        } 
        
        
    };

    $(function() {
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.atiAddress('<%=this.ID %>');
        Aqufit.Page.<%=this.ID %>.gmapLoad();
        $('#<%=ddlCountry.ClientID %>').change(function(){
            Aqufit.Page.<%=this.ID %>.PopulateRegionsFromCountry();
        });
        $('#<%=atiTxtPostal.ClientID %>, #<%=atiTxtAddress.ClientID %>, #<%=atiTxtCity.ClientID %>, #<%=ddlCountry.ClientID %>,  #<%=ddlRegion.ClientID %>, #<%=txtRegion.ClientID %>').change(function(){            
            Aqufit.Page.<%=this.ID %>.centerToAddress();
        });
    });
    
</script>

    <asp:HiddenField ID="hiddenLat" runat="server" />
    <asp:HiddenField ID="hiddenLng" runat="server" />
    <asp:HiddenField ID="HiddenHomeLat" runat="server" />
    <asp:HiddenField ID="HiddenHomeLng" runat="server" />    
    <dt><asp:Label id="plPostal" runat="server" controlname="txtPostal" text="Postal/Zip:" /></dt>
    <dd> 
        <asp:TextBox ID="atiTxtPostal" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="63" />
        <asp:RequiredFieldValidator ID="rfvPostal" runat="server" ValidationGroup="Address" ControlToValidate="atiTxtPostal" ErrorMessage="Postal required!" Text="*" />		
    </dd> 
    
    <dt><asp:Label id="plCountry" runat="server" controlname="ddlCountry" text="Country:" /></dt>
    <dd>  
	    <asp:DropDownList id="ddlCountry" runat="server" cssclass="ui-corner-all ui-widget-content atiTxtBox" DataValueField="Value" DataTextField="Text"></asp:DropDownList>		   	    
    </dd>
    <dt><asp:Label id="plRegion" runat="server" controlname="cboRegion" text="Region:" /></dt>	 
    <dd>    
        <asp:DropDownList id="ddlRegion" runat="server" cssclass="ui-corner-all ui-widget-content atiTxtBox" DataValueField="Value" DataTextField="Text"></asp:DropDownList>
        <asp:textbox id="txtRegion" runat="server" MaxLength="100" cssclass="ui-corner-all ui-widget-content atiTxtBox" style="display: none;"></asp:textbox>	    		
    </dd>	
    <dt><asp:Label id="plCity" runat="server" controlname="txtCity" text="City:" /></dt>
    <dd>
        <asp:TextBox ID="atiTxtCity" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="63" />
        <asp:RequiredFieldValidator ID="rfvCity" runat="server" ValidationGroup="Address" ControlToValidate="atiTxtCity" ErrorMessage="City required!" Text="*" />	    
    </dd>

    <dt><asp:Label id="plStreet" runat="server" controlname="txtStreet" text="Street:" /></dt>
    <dd>    
        <asp:TextBox ID="atiTxtAddress" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="127" />
        <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ValidationGroup="Address" ControlToValidate="atiTxtAddress" ErrorMessage="Street required!" Text="*" />
	</dd>        

    <dt id="dtLocation"><asp:Label id="plMapPos" runat="server" controlname="txtMapPos" text="Location:" /></dt>		    	    
	<dd id="ddLocation"><div id="map_canvas" style="height: 250px; width: 300px; border: 1px solid #AAAAAA;"></div></dd>

