using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DesktopModules_ATI_Base_controls_ATI_TextBoxControl : Affine.Web.Controls.ATI_ValidationBaseControl
{
    public short TabIndex
    {
        get { return atiTextBox.TabIndex; }
        set { atiTextBox.TabIndex = value; }
    }
    

    public string CssClass
    {
        get { return atiTextBox.CssClass; }
        set { atiTextBox.CssClass = value; }
    }

    public int MaxLength
    {
        get { return atiTextBox.MaxLength; }
        set { atiTextBox.MaxLength = value; }
    }

    public TextBoxMode TextMode
    {
        get { return atiTextBox.TextMode; }
        set { atiTextBox.TextMode = value; }
    }

    public string Text
    {
        get { return atiTextBox.Text; }
        set { atiTextBox.Text = value; }
    }

    private bool _IsDouble = false;
    public bool IsDouble
    {
        get { return _IsDouble; }
        set { _IsDouble = value; }
    }

    private bool _IsRequired = false;
    public bool IsRequired
    {
        get { return _IsRequired; }
        set { _IsRequired = value; }
    }

    private bool _HideCheck = false;
    public bool HideCheck
    {
        get { return _HideCheck; }
        set { _HideCheck = value; }
    }

    private int _MinLength = -1;
    public int MinLength
    {
        get { return _MinLength; }
        set { _MinLength = value; }
    }

    private string _RegEx = string.Empty;
    public string RegEx
    {
        get { return _RegEx; }
        set { _RegEx = value; }
    }

    private string _RegExErrorMsg = string.Empty;
    public string RegExErrorMsg
    {
        get { return _RegExErrorMsg; }
        set { _RegExErrorMsg = value; }
    }

    private string GetAtiTextBoxJs()
    {
        string ret = string.Empty;
        // CA - bailing on the check box for CORRECT input... I will leave the code in but I am just passing NULL image.
        //ret += "var " + this.ID + " = new atiTextBox('" + atiTextBox.ClientID + "','" + atiTextBox_checkImg.ClientID + "','" + RadToolTip1.ClientID + "','" + atiLabel.ClientID + "'," + (this.IsRequired ? "true" : "false") + "," + (this.IsDouble ? "true" : "false") + "," + this.MinLength + ");";
        ret += "var " + this.ID + " = new atiTextBox('" + atiTextBox.ClientID + "','','" + RadToolTip1.ClientID + "','" + atiLabel.ClientID + "'," + (this.IsRequired ? "true" : "false") + "," + (this.IsDouble ? "true" : "false") + "," + this.MinLength + "); \n";
        if (this.RegEx != string.Empty) ret += this.ID + ".setRegEx('" + this.RegEx + "','" + this.RegExErrorMsg + "'); \n";
        ret += "_atiTextObjectCollection['"+atiTextBox.ClientID+"'] = " + this.ID + "; \n";
        return ret;
    }

    private string GetAtiTextBoxObjectJs()
    {
        string ret = string.Empty;
        ret += "var _atiTextObjectCollection = []; ";
        ret += "function atiTextBox(txtId, checkId, toolId, errId, isRequired, isDouble, minLen) {";
        ret +=      "this.domText = document.getElementById(txtId); ";
        ret +=      "this.domError = document.getElementById(errId); ";
        ret +=      "this.toolTipId = toolId; ";
        ret +=      "this.isRequired = isRequired; ";
        ret +=      "this.isDouble = isDouble; ";
        ret +=      "this.minLength = minLen; ";
        ret +=      "this.domCheckImg = document.getElementById(checkId);";
        ret +=      "this.errorMsg = '';";
        ret +=      "this.regEx = '';";
        ret +=      "this.regExErrStr = '';";
        ret +=      "if (this.domText) { ";
        ret +=          "if (isDouble) { ";
        ret +=              "this.domText.onkeypress = function(evt) { ";
        ret +=                  "var target = ''; ";
        ret +=                  "if (!evt) { ";
        ret +=                      "evt = window.event;";
        ret +=                      "target = evt.srcElement;";
        ret +=                  "} else {";
        ret +=                      "target = evt.target;";
        ret +=                  "}";
        ret +=                  "var charCode = (evt.which) ? evt.which : event.keyCode;";
        ret +=                  "var ch = String.fromCharCode(charCode);";
        ret +=                  "if ((ch == '.')) {";
        ret +=                      "return true;";
        ret +=                  "}";
        ret +=                  "if (charCode > 31 && (charCode < 48 || charCode > 57))";
        ret +=                      "return false;";
        ret +=                  "return true;";
        ret +=          "}";
        ret +=  "}";
        ret +=  "this.domText.onchange = function(event) {";
        ret +=      "var target = '';";
        ret +=      "if (!event) {";
        ret +=          "event = window.event;";
        ret +=          "target = event.srcElement;";
        ret +=      "} else {";
        ret +=          "target = event.target;";
        ret +=      "}";
        ret +=      "var atiTxt = _atiTextObjectCollection[target.id];";
        ret +=      "if (atiTxt.domCheckImg) atiTxt.domCheckImg.style.visibility = 'hidden';";
        ret +=      "var valid = atiTxt.validate();";
        ret +=      "if (valid) {";
        ret +=          "if (atiTxt.domCheckImg) atiTxt.domCheckImg.style.visibility = 'visible';";
        ret +=      "}";
        ret += "}";
        ret +="}";
        ret +="};";

        ret +="atiTextBox.prototype = {";
        ret +=    "value: function() {";
        ret +=        "return this.domText.value;";
        ret +=    "},";

        ret +=     "setRegEx: function(regEx, errStr) {";
        ret +=        "this.regExErrStr = errStr;";
        ret +=        "this.regEx = regEx;";
        ret +=    "},";

        ret +=    "validate: function() {";
        ret +=        "var toolTip = $find(this.toolTipId);";
        ret +=        "toolTip.hide();";
        ret +=        "if (this.isRequired) {";
        ret +=             "if (this.value() == '') {";
        ret +=                "this.errorMsg = 'required field.';";
        ret +=                "this.domError.innerHTML = this.errorMsg;";
        ret +=                "toolTip.show();";
        ret +=                "return false;";
        ret +=            "}";
        ret +=        "}";
        ret +=        "if (this.isDouble) {";
        ret +=            "if (!parseFloat(this.value())) {";
        ret +=                "this.errorMsg = 'must be a Real Number.';";
        ret +=                "this.domError.innerHTML = this.errorMsg;";
        ret +=                "toolTip.show();";
        ret +=                "return false;";
        ret +=            "}";
        ret +=        "}";
        ret +=        "if (this.minLength > 0) {";
        ret +=            "if (this.value().length < this.minLength) {";
        ret +=                "this.errorMsg = 'must be at least ' + this.minLength + ' characters.';";
        ret +=                "this.domError.innerHTML = this.errorMsg;";
        ret +=                "toolTip.show();";
        ret +=                "return false;";
        ret +=            "}";
        ret +=        "}";
        ret +=        "if (this.regEx != '') {";
        ret +=            "var re = new RegExp(this.regEx);";
        ret +=            "var m = re.exec(this.value());";
        ret +=            "if (m == null) {";
        ret +=                "this.errorMsg = this.regExErrStr;";
        ret +=                "this.domError.innerHTML = this.errorMsg;";
        ret +=                "toolTip.show();";
        ret +=                "return false;";
        ret +=            "}";
        ret +=        "}";
        ret +=        "return true;";
        ret +=    "}";
        ret +="};";

        return ret;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            imgError.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iError.png");
            atiTextBox_checkImg.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCheck.png");
            atiTextBox_checkImg.Visible = !this.HideCheck;
        }
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "atiTxtValidation" + this.ID, GetAtiTextBoxJs(), true);
    }
}
