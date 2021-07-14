<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_Settings.master" AutoEventWireup="true" CodeFile="Adm_SchoolSetup.aspx.cs" Inherits="Adm_SchoolSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>
    <div runat="server" id="school_section">
        <div class="cont_right formWrapper">
            <h4><b>Institution Configurations</b></h4>

            <p class="note">Fields with <span class="required">*</span>are required.</p>
            <div id="div_error_summary" class="errorSummary" runat="server" visible="false">
                <p>Please fix the following input errors:</p>
                <ul>
                    <li>dummy</li>
                </ul>
            </div>
             <div class="formCon">
            <!-- School Information -->
            <div class="formCon_os">
                <div class="formConInner">
                    <h3>School Information</h3>




                    <table style="width:100%" border="0" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>

                                <td style="width:33%">
                                    <label>School / College Name</label>                                    
                                    <asp:TextBox ID="tbx_collegename" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                                <td style="width:33%">
                                    <label>Registration ID</label>                                    
                                    <asp:TextBox ID="tbx_registrationid" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                                <td style="width:33%">
                                    <label>Founded On</label>                                    
                                    <asp:TextBox ID="tbx_founded" runat="server" AutoCompleteType="Disabled" MaxLength="10" class="hasDatepicker"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="3" class="td_1">
                                    <label>Address</label>                                    
                                    <textarea style="width: 100% !important; height: 150px;" runat="server" id="tbx_address"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Curriculum</label>                                    
                                     <asp:TextBox ID="tbx_curriculam" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                                <td>
                                    <label>Zip code</label>                                    
                                     <asp:TextBox ID="tbx_zipcode" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                                <td>
                                    <label>Phone</label>                                    
                                     <asp:TextBox ID="tbx_phone1" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Alternate Phone</label>                                    
                                     <asp:TextBox ID="tbx_phone2" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                                <td>
                                    <label>Email</label>                                    
                                     <asp:TextBox ID="tbx_email" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                                <td>
                                    <label>Fax</label>                                   
                                     <asp:TextBox ID="tbx_fax" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_1">
                                    <label>Upload Logo</label>
                                    <div class="formCon_os_infpbox">                                        
                                        <asp:HiddenField ID="hd_uploadfile" runat="server" />
                                        <input id="fl_Logo_uploadedFile" type="file" runat="server">
                                        <div class="errorMessage" id="err_Logo_uploadedFile" runat="server" visible="false"></div>
                                        <p><strong><i>(supported formats : .jpg , .png ; max filesize: 60Kb,recommended size : 64*64)</i></strong></p>
                                    </div>

                                </td>

                            </tr>
                            <tr>
                                <td class="td_1">
                                    <label>Favicon</label>
                                    <div class="formCon_os_infpbox">
                                        <br>
                                        
                                        <asp:HiddenField ID="hd_favicon" runat="server" />
                                        <input id="fl_icon" onchange="validate(this.value)" type="file" runat="server">
                                        <div class="errorMessage" id="err_Favicon_icon"  runat="server" visible="false"></div>
                                        <p><strong><i>(supported formats : .ico, .jpg recommended size : 16*16)</i></strong></p>
                                    </div>

                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <!-- End School Information -->

            <!-- Head of institution -->
            <div class="formCon_os">
                <div class="formConInner">
                    <h3>Principal / Head of the Institution</h3>
                    <table style="width:100%" border="0" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td>
                                    <label>Name of the Principal</label>                                    
                                     <asp:TextBox ID="tbx_principalname" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                                <td>
                                    <label>Email of the Principal</label>                                    
                                     <asp:TextBox ID="tbx_principalemail" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                                <td>
                                    <label>Phone</label>                                    
                                     <asp:TextBox ID="tbx_principalphone" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>

                                <td>
                                    <label>Mobile</label>                                    
                                    <asp:TextBox ID="tbx_principalmobile" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <!-- End Head of institution -->

            <!-- Academic Year Setup -->
            <div class="formCon_os">
                <div class="formConInner">
                    <h3>School Year Setup</h3>
                    <table style="width:100%" border="0" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td class="td_1">
                                    <a class="a_tag-btn" href="Adm_academicYears.aspx?mode=add"><b>Create New Academic Year</b></a>                 </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Academic Year <span class="required">*</span></label>
                                    <asp:DropDownList ID="drp_academic_year" runat="server"></asp:DropDownList>                                    

                                </td>
                                <td>
                                    <label>Finance Year Start</label>
                                    <input style="" readonly="readonly" id="lbl_financial_yr_start" type="text" runat="server" class="hasDatepicker">
                                   
                                </td>
                                <td>
                                    <label>Finance Year End</label>                                    
                                    <input style="" readonly="readonly" id="lbl_financial_yr_end" type="text" runat="server" class="hasDatepicker">
                                </td>

                            </tr>




                        </tbody>
                    </table>
                </div>
            </div>
            <!-- End Academic Year Setup -->

            <!-- Other Settings -->
            <div class="formCon_os">
                <div class="formConInner">
                    <h3>Application Settings</h3>
                    <table style="width:100%" border="0" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td>
                                    <label>
                                        Currency  </label>                  
                                            <select id="sel_currency" runat="server">
                                                <option value="EUR">EUR</option>
                                                <option value="AED">AED</option>
                                                <option value="AFN">AFN</option>
                                                <option value="XCD">XCD</option>
                                                <option value="ALL">ALL</option>
                                                <option value="AMD">AMD</option>
                                                <option value="AOA">AOA</option>
                                                <option value="ARS">ARS</option>
                                                <option value="USD">USD</option>
                                                <option value="AUD">AUD</option>
                                                <option value="AWG">AWG</option>
                                                <option value="AZN">AZN</option>
                                                <option value="BAM">BAM</option>
                                                <option value="BBD">BBD</option>
                                                <option value="BDT">BDT</option>
                                                <option value="XOF">XOF</option>
                                                <option value="BGN">BGN</option>
                                                <option value="BHD">BHD</option>
                                                <option value="BIF">BIF</option>
                                                <option value="BMD">BMD</option>
                                                <option value="BND">BND</option>
                                                <option value="BOB">BOB</option>
                                                <option value="BRL">BRL</option>
                                                <option value="BSD">BSD</option>
                                                <option value="BTN">BTN</option>
                                                <option value="NOK">NOK</option>
                                                <option value="BWP">BWP</option>
                                                <option value="BYR">BYR</option>
                                                <option value="BZD">BZD</option>
                                                <option value="CAD">CAD</option>
                                                <option value="CDF">CDF</option>
                                                <option value="XAF">XAF</option>
                                                <option value="CHF">CHF</option>
                                                <option value="NZD">NZD</option>
                                                <option value="CLP">CLP</option>
                                                <option value="CNY">CNY</option>
                                                <option value="COP">COP</option>
                                                <option value="CRC">CRC</option>
                                                <option value="CUP">CUP</option>
                                                <option value="CVE">CVE</option>
                                                <option value="ANG">ANG</option>
                                                <option value="CZK">CZK</option>
                                                <option value="DJF">DJF</option>
                                                <option value="DKK">DKK</option>
                                                <option value="DOP">DOP</option>
                                                <option value="DZD">DZD</option>
                                                <option value="EGP">EGP</option>
                                                <option value="MAD">MAD</option>
                                                <option value="ERN">ERN</option>
                                                <option value="ETB">ETB</option>
                                                <option value="FJD">FJD</option>
                                                <option value="FKP">FKP</option>
                                                <option value="GBP">GBP</option>
                                                <option value="GEL">GEL</option>
                                                <option value="GHS">GHS</option>
                                                <option value="GIP">GIP</option>
                                                <option value="GMD">GMD</option>
                                                <option value="GNF">GNF</option>
                                                <option value="GTQ">GTQ</option>
                                                <option value="GYD">GYD</option>
                                                <option value="HKD">HKD</option>
                                                <option value="HNL">HNL</option>
                                                <option value="HRK">HRK</option>
                                                <option value="HTG">HTG</option>
                                                <option value="HUF">HUF</option>
                                                <option value="IDR">IDR</option>
                                                <option value="ILS">ILS</option>
                                                <option value="INR" selected="selected">INR</option>
                                                <option value="IQD">IQD</option>
                                                <option value="IRR">IRR</option>
                                                <option value="ISK">ISK</option>
                                                <option value="JMD">JMD</option>
                                                <option value="JOD">JOD</option>
                                                <option value="JPY">JPY</option>
                                                <option value="KES">KES</option>
                                                <option value="KGS">KGS</option>
                                                <option value="KHR">KHR</option>
                                                <option value="KMF">KMF</option>
                                                <option value="KPW">KPW</option>
                                                <option value="KRW">KRW</option>
                                                <option value="KWD">KWD</option>
                                                <option value="KYD">KYD</option>
                                                <option value="KZT">KZT</option>
                                                <option value="LAK">LAK</option>
                                                <option value="LBP">LBP</option>
                                                <option value="LKR">LKR</option>
                                                <option value="LRD">LRD</option>
                                                <option value="LSL">LSL</option>
                                                <option value="LTL">LTL</option>
                                                <option value="LYD">LYD</option>
                                                <option value="MDL">MDL</option>
                                                <option value="MGA">MGA</option>
                                                <option value="MKD">MKD</option>
                                                <option value="MMK">MMK</option>
                                                <option value="MNT">MNT</option>
                                                <option value="MOP">MOP</option>
                                                <option value="MRO">MRO</option>
                                                <option value="MUR">MUR</option>
                                                <option value="MVR">MVR</option>
                                                <option value="MWK">MWK</option>
                                                <option value="MXN">MXN</option>
                                                <option value="MYR">MYR</option>
                                                <option value="MZN">MZN</option>
                                                <option value="NAD">NAD</option>
                                                <option value="XPF">XPF</option>
                                                <option value="NGN">NGN</option>
                                                <option value="NIO">NIO</option>
                                                <option value="NPR">NPR</option>
                                                <option value="OMR">OMR</option>
                                                <option value="PAB">PAB</option>
                                                <option value="PEN">PEN</option>
                                                <option value="PGK">PGK</option>
                                                <option value="PHP">PHP</option>
                                                <option value="PKR">PKR</option>
                                                <option value="PLN">PLN</option>
                                                <option value="PYG">PYG</option>
                                                <option value="QAR">QAR</option>
                                                <option value="RON">RON</option>
                                                <option value="RSD">RSD</option>
                                                <option value="RUB">RUB</option>
                                                <option value="RWF">RWF</option>
                                                <option value="SAR">SAR</option>
                                                <option value="SBD">SBD</option>
                                                <option value="SCR">SCR</option>
                                                <option value="SDG">SDG</option>
                                                <option value="SEK">SEK</option>
                                                <option value="SGD">SGD</option>
                                                <option value="SHP">SHP</option>
                                                <option value="SLL">SLL</option>
                                                <option value="SOS">SOS</option>
                                                <option value="SRD">SRD</option>
                                                <option value="SSP">SSP</option>
                                                <option value="STD">STD</option>
                                                <option value="SYP">SYP</option>
                                                <option value="SZL">SZL</option>
                                                <option value="THB">THB</option>
                                                <option value="TJS">TJS</option>
                                                <option value="TMT">TMT</option>
                                                <option value="TND">TND</option>
                                                <option value="TOP">TOP</option>
                                                <option value="TRY">TRY</option>
                                                <option value="TTD">TTD</option>
                                                <option value="TWD">TWD</option>
                                                <option value="TZS">TZS</option>
                                                <option value="UAH">UAH</option>
                                                <option value="UGX">UGX</option>
                                                <option value="UYU">UYU</option>
                                                <option value="UZS">UZS</option>
                                                <option value="VEF">VEF</option>
                                                <option value="VND">VND</option>
                                                <option value="VUV">VUV</option>
                                                <option value="WST">WST</option>
                                                <option value="YER">YER</option>
                                                <option value="ZAR">ZAR</option>
                                                <option value="ZMW">ZMW</option>
                                                <option value="ZWL">ZWL</option>
                                            </select>
                                   
                                </td>
                                <td>
                                    <label>Language</label>
                                    <select id="sel_language" runat="server">
                                        <option value="en_us" selected="selected">English</option>
                                        <option value="af">Afrikaans</option>
                                        <option value="sq">shqiptar</option>
                                        <option value="ar">العربية</option>
                                        <option value="cz">中国的 </option>
                                        <option value="cs">český</option>
                                        <option value="nl">Nederlands</option>
                                        <option value="fr">français</option>
                                        <option value="de">Deutsch</option>
                                        <option value="el">ελληνικά</option>
                                        <option value="gu">Γκουτζαρατικά</option>
                                        <option value="hi">हिंदी</option>
                                        <option value="id">Indonesia</option>
                                        <option value="ga">Gaeilge</option>
                                        <option value="it">italiano</option>
                                        <option value="ja">日本人</option>
                                        <option value="kn">ಕನ್ನಡ</option>
                                        <option value="ko">한국의</option>
                                        <option value="la">Latine</option>
                                        <option value="ms">Melayu</option>
                                        <option value="pt">português</option>
                                        <option value="ru">русский</option>
                                        <option value="es">español</option>
                                        <option value="ta">தமிழ்</option>
                                        <option value="te">తెలుగు</option>
                                        <option value="th">ภาษาไทย</option>
                                        <option value="uk">Український</option>
                                        <option value="ur">اردو</option>
                                        <option value="vi">Việt</option>
                                        <option value="vi_vn">Tiếng Việt</option>
                                    </select>
                                </td>
                                <td>
                                    <label>Date Format</label>
                                    <select id="sel_dateformat" runat="server">
                                        <option value="m/d/yy">12/18/2019</option>
                                        <option value="M d.yy">Dec 18.2019</option>
                                        <option value="D, M d.yy">Wed, Dec 18.2019</option>
                                        <option value="d M yy" selected="selected">18 Dec 2019</option>
                                        <option value="yy/m/d">2019/12/18</option>
                                    </select>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <label>Time Zone</label>
                                    <select  id="sel_timezone" runat="server">
                                        <option value="2">Africa/Abidjan</option>
                                        <option value="3">Africa/Accra</option>
                                        <option value="4">Africa/Addis_Ababa</option>
                                        <option value="5">Africa/Algiers</option>
                                        <option value="6">Africa/Asmara</option>
                                        <option value="7">Africa/Asmera</option>
                                        <option value="8">Africa/Bamako</option>
                                        <option value="9">Africa/Bangui</option>
                                        <option value="10">Africa/Banjul</option>
                                        <option value="11">Africa/Bissau</option>
                                        <option value="12">Africa/Blantyre</option>
                                        <option value="13">Africa/Brazzaville</option>
                                        <option value="14">Africa/Bujumbura</option>
                                        <option value="15">Africa/Cairo</option>
                                        <option value="16">Africa/Casablanca</option>
                                        <option value="17">Africa/Ceuta</option>
                                        <option value="18">Africa/Conakry</option>
                                        <option value="19">Africa/Dakar</option>
                                        <option value="20">Africa/Dar_es_Salaam</option>
                                        <option value="21">Africa/Djibouti</option>
                                        <option value="22">Africa/Douala</option>
                                        <option value="23">Africa/El_Aaiun</option>
                                        <option value="24">Africa/Freetown</option>
                                        <option value="25">Africa/Gaborone</option>
                                        <option value="26">Africa/Harare</option>
                                        <option value="27">Africa/Johannesburg</option>
                                        <option value="28">Africa/Kampala</option>
                                        <option value="29">Africa/Khartoum</option>
                                        <option value="30">Africa/Kigali</option>
                                        <option value="31">Africa/Kinshasa</option>
                                        <option value="32">Africa/Lagos</option>
                                        <option value="33">Africa/Libreville</option>
                                        <option value="34">Africa/Lome</option>
                                        <option value="35">Africa/Luanda</option>
                                        <option value="36">Africa/Lubumbashi</option>
                                        <option value="37">Africa/Lusaka</option>
                                        <option value="38">Africa/Malabo</option>
                                        <option value="39">Africa/Maputo</option>
                                        <option value="40">Africa/Maseru</option>
                                        <option value="41">Africa/Mbabane</option>
                                        <option value="42">Africa/Mogadishu</option>
                                        <option value="43">Africa/Monrovia</option>
                                        <option value="44">Africa/Nairobi</option>
                                        <option value="45">Africa/Ndjamena</option>
                                        <option value="46">Africa/Niamey</option>
                                        <option value="47">Africa/Nouakchott</option>
                                        <option value="48">Africa/Ouagadougou</option>
                                        <option value="49">Africa/Porto-Novo</option>
                                        <option value="50">Africa/Sao_Tome</option>
                                        <option value="51">Africa/Timbuktu</option>
                                        <option value="52">Africa/Tripoli</option>
                                        <option value="53">Africa/Tunis</option>
                                        <option value="54">Africa/Windhoek</option>
                                        <option value="55">America/Adak</option>
                                        <option value="56">America/Anchorage</option>
                                        <option value="57">America/Anguilla</option>
                                        <option value="58">America/Antigua</option>
                                        <option value="59">America/Araguaina</option>
                                        <option value="60">America/Argentina/Buenos_Aires</option>
                                        <option value="61">America/Argentina/Catamarca</option>
                                        <option value="62">America/Argentina/ComodRivadavia</option>
                                        <option value="63">America/Argentina/Cordoba</option>
                                        <option value="64">America/Argentina/Jujuy</option>
                                        <option value="65">America/Argentina/La_Rioja</option>
                                        <option value="66">America/Argentina/Mendoza</option>
                                        <option value="67">America/Argentina/Rio_Gallegos</option>
                                        <option value="68">America/Argentina/Salta</option>
                                        <option value="69">America/Argentina/San_Juan</option>
                                        <option value="70">America/Argentina/San_Luis</option>
                                        <option value="71">America/Argentina/Tucuman</option>
                                        <option value="72">America/Argentina/Ushuaia</option>
                                        <option value="73">America/Aruba</option>
                                        <option value="74">America/Asuncion</option>
                                        <option value="75">America/Atikokan</option>
                                        <option value="76">America/Atka</option>
                                        <option value="77">America/Bahia</option>
                                        <option value="78">America/Bahia_Banderas</option>
                                        <option value="79">America/Barbados</option>
                                        <option value="80">America/Belem</option>
                                        <option value="81">America/Belize</option>
                                        <option value="82">America/Blanc-Sablon</option>
                                        <option value="83">America/Boa_Vista</option>
                                        <option value="84">America/Bogota</option>
                                        <option value="85">America/Boise</option>
                                        <option value="86">America/Buenos_Aires</option>
                                        <option value="87">America/Cambridge_Bay</option>
                                        <option value="88">America/Campo_Grande</option>
                                        <option value="89">America/Cancun</option>
                                        <option value="90">America/Caracas</option>
                                        <option value="91">America/Catamarca</option>
                                        <option value="92">America/Cayenne</option>
                                        <option value="93">America/Cayman</option>
                                        <option value="94">America/Chicago</option>
                                        <option value="95">America/Chihuahua</option>
                                        <option value="96">America/Coral_Harbour</option>
                                        <option value="97">America/Cordoba</option>
                                        <option value="98">America/Costa_Rica</option>
                                        <option value="99">America/Cuiaba</option>
                                        <option value="100">America/Curacao</option>
                                        <option value="101">America/Danmarkshavn</option>
                                        <option value="102">America/Dawson</option>
                                        <option value="103">America/Dawson_Creek</option>
                                        <option value="104">America/Denver</option>
                                        <option value="105">America/Detroit</option>
                                        <option value="106">America/Dominica</option>
                                        <option value="107">America/Edmonton</option>
                                        <option value="108">America/Eirunepe</option>
                                        <option value="109">America/El_Salvador</option>
                                        <option value="110">America/Ensenada</option>
                                        <option value="111">America/Fortaleza</option>
                                        <option value="112">America/Fort_Wayne</option>
                                        <option value="113">America/Glace_Bay</option>
                                        <option value="114">America/Godthab</option>
                                        <option value="115">America/Goose_Bay</option>
                                        <option value="116">America/Grand_Turk</option>
                                        <option value="117">America/Grenada</option>
                                        <option value="118">America/Guadeloupe</option>
                                        <option value="119">America/Guatemala</option>
                                        <option value="120">America/Guayaquil</option>
                                        <option value="121">America/Guyana</option>
                                        <option value="122">America/Halifax</option>
                                        <option value="123">America/Havana</option>
                                        <option value="124">America/Hermosillo</option>
                                        <option value="125">America/Indiana/Indianapolis</option>
                                        <option value="126">America/Indiana/Knox</option>
                                        <option value="127">America/Indiana/Marengo</option>
                                        <option value="128">America/Indiana/Petersburg</option>
                                        <option value="129">America/Indianapolis</option>
                                        <option value="130">America/Indiana/Tell_City</option>
                                        <option value="131">America/Indiana/Vevay</option>
                                        <option value="132">America/Indiana/Vincennes</option>
                                        <option value="133">America/Indiana/Winamac</option>
                                        <option value="134">America/Inuvik</option>
                                        <option value="135">America/Iqaluit</option>
                                        <option value="136">America/Jamaica</option>
                                        <option value="137">America/Jujuy</option>
                                        <option value="138">America/Juneau</option>
                                        <option value="139">America/Kentucky/Louisville</option>
                                        <option value="140">America/Kentucky/Monticello</option>
                                        <option value="141">America/Knox_IN</option>
                                        <option value="142">America/La_Paz</option>
                                        <option value="143">America/Lima</option>
                                        <option value="144">America/Los_Angeles</option>
                                        <option value="145">America/Louisville</option>
                                        <option value="146">America/Maceio</option>
                                        <option value="147">America/Managua</option>
                                        <option value="148">America/Manaus</option>
                                        <option value="149">America/Marigot</option>
                                        <option value="150">America/Martinique</option>
                                        <option value="151">America/Matamoros</option>
                                        <option value="152">America/Mazatlan</option>
                                        <option value="153">America/Mendoza</option>
                                        <option value="154">America/Menominee</option>
                                        <option value="155">America/Merida</option>
                                        <option value="156">America/Metlakatla</option>
                                        <option value="157">America/Mexico_City</option>
                                        <option value="158">America/Miquelon</option>
                                        <option value="159">America/Moncton</option>
                                        <option value="160">America/Monterrey</option>
                                        <option value="161">America/Montevideo</option>
                                        <option value="162">America/Montreal</option>
                                        <option value="163">America/Montserrat</option>
                                        <option value="164">America/Nassau</option>
                                        <option value="165">America/New_York</option>
                                        <option value="166">America/Nipigon</option>
                                        <option value="167">America/Nome</option>
                                        <option value="168">America/Noronha</option>
                                        <option value="169">America/North_Dakota/Beulah</option>
                                        <option value="170">America/North_Dakota/Center</option>
                                        <option value="171">America/North_Dakota/New_Salem</option>
                                        <option value="172">America/Ojinaga</option>
                                        <option value="173">America/Panama</option>
                                        <option value="174">America/Pangnirtung</option>
                                        <option value="175">America/Paramaribo</option>
                                        <option value="176">America/Phoenix</option>
                                        <option value="177">America/Port-au-Prince</option>
                                        <option value="178">America/Porto_Acre</option>
                                        <option value="179">America/Port_of_Spain</option>
                                        <option value="180">America/Porto_Velho</option>
                                        <option value="181">America/Puerto_Rico</option>
                                        <option value="182">America/Rainy_River</option>
                                        <option value="183">America/Rankin_Inlet</option>
                                        <option value="184">America/Recife</option>
                                        <option value="185">America/Regina</option>
                                        <option value="186">America/Resolute</option>
                                        <option value="187">America/Rio_Branco</option>
                                        <option value="188">America/Rosario</option>
                                        <option value="189">America/Santa_Isabel</option>
                                        <option value="190">America/Santarem</option>
                                        <option value="191">America/Santiago</option>
                                        <option value="192">America/Santo_Domingo</option>
                                        <option value="193">America/Sao_Paulo</option>
                                        <option value="194">America/Scoresbysund</option>
                                        <option value="195">America/Shiprock</option>
                                        <option value="196">America/Sitka</option>
                                        <option value="197">America/St_Barthelemy</option>
                                        <option value="198">America/St_Johns</option>
                                        <option value="199">America/St_Kitts</option>
                                        <option value="200">America/St_Lucia</option>
                                        <option value="201">America/St_Thomas</option>
                                        <option value="202">America/St_Vincent</option>
                                        <option value="203">America/Swift_Current</option>
                                        <option value="204">America/Tegucigalpa</option>
                                        <option value="205">America/Thule</option>
                                        <option value="206">America/Thunder_Bay</option>
                                        <option value="207">America/Tijuana</option>
                                        <option value="208">America/Toronto</option>
                                        <option value="209">America/Tortola</option>
                                        <option value="210">America/Vancouver</option>
                                        <option value="211">America/Virgin</option>
                                        <option value="212">America/Whitehorse</option>
                                        <option value="213">America/Winnipeg</option>
                                        <option value="214">America/Yakutat</option>
                                        <option value="215">America/Yellowknife</option>
                                        <option value="216">Antarctica/Casey</option>
                                        <option value="217">Antarctica/Davis</option>
                                        <option value="218">Antarctica/DumontDUrville</option>
                                        <option value="219">Antarctica/Macquarie</option>
                                        <option value="220">Antarctica/Mawson</option>
                                        <option value="221">Antarctica/McMurdo</option>
                                        <option value="222">Antarctica/Palmer</option>
                                        <option value="223">Antarctica/Rothera</option>
                                        <option value="224">Antarctica/South_Pole</option>
                                        <option value="225">Antarctica/Syowa</option>
                                        <option value="226">Antarctica/Vostok</option>
                                        <option value="227">Arctic/Longyearbyen</option>
                                        <option value="228">Asia/Aden</option>
                                        <option value="229">Asia/Almaty</option>
                                        <option value="230">Asia/Amman</option>
                                        <option value="231">Asia/Anadyr</option>
                                        <option value="232">Asia/Aqtau</option>
                                        <option value="233">Asia/Aqtobe</option>
                                        <option value="234">Asia/Ashgabat</option>
                                        <option value="235">Asia/Ashkhabad</option>
                                        <option value="236">Asia/Baghdad</option>
                                        <option value="237">Asia/Bahrain</option>
                                        <option value="238">Asia/Baku</option>
                                        <option value="239">Asia/Bangkok</option>
                                        <option value="240">Asia/Beirut</option>
                                        <option value="241">Asia/Bishkek</option>
                                        <option value="242">Asia/Brunei</option>
                                        <option value="243">Asia/Calcutta</option>
                                        <option value="244">Asia/Choibalsan</option>
                                        <option value="245">Asia/Chongqing</option>
                                        <option value="246">Asia/Chungking</option>
                                        <option value="247">Asia/Colombo</option>
                                        <option value="248">Asia/Dacca</option>
                                        <option value="249">Asia/Damascus</option>
                                        <option value="250">Asia/Dhaka</option>
                                        <option value="251">Asia/Dili</option>
                                        <option value="252">Asia/Dubai</option>
                                        <option value="253">Asia/Dushanbe</option>
                                        <option value="254">Asia/Gaza</option>
                                        <option value="255">Asia/Harbin</option>
                                        <option value="256">Asia/Ho_Chi_Minh</option>
                                        <option value="257">Asia/Hong_Kong</option>
                                        <option value="258">Asia/Hovd</option>
                                        <option value="259">Asia/Irkutsk</option>
                                        <option value="260">Asia/Istanbul</option>
                                        <option value="261">Asia/Jakarta</option>
                                        <option value="262">Asia/Jayapura</option>
                                        <option value="263">Asia/Jerusalem</option>
                                        <option value="264">Asia/Kabul</option>
                                        <option value="265">Asia/Kamchatka</option>
                                        <option value="266">Asia/Karachi</option>
                                        <option value="267">Asia/Kashgar</option>
                                        <option value="268">Asia/Kathmandu</option>
                                        <option value="269">Asia/Katmandu</option>
                                        <option value="270" selected="selected">Asia/Kolkata</option>
                                        <option value="271">Asia/Krasnoyarsk</option>
                                        <option value="272">Asia/Kuala_Lumpur</option>
                                        <option value="273">Asia/Kuching</option>
                                        <option value="274">Asia/Kuwait</option>
                                        <option value="275">Asia/Macao</option>
                                        <option value="276">Asia/Macau</option>
                                        <option value="277">Asia/Magadan</option>
                                        <option value="278">Asia/Makassar</option>
                                        <option value="279">Asia/Manila</option>
                                        <option value="280">Asia/Muscat</option>
                                        <option value="281">Asia/Nicosia</option>
                                        <option value="282">Asia/Novokuznetsk</option>
                                        <option value="283">Asia/Novosibirsk</option>
                                        <option value="284">Asia/Omsk</option>
                                        <option value="285">Asia/Oral</option>
                                        <option value="286">Asia/Phnom_Penh</option>
                                        <option value="287">Asia/Pontianak</option>
                                        <option value="288">Asia/Pyongyang</option>
                                        <option value="289">Asia/Qatar</option>
                                        <option value="290">Asia/Qyzylorda</option>
                                        <option value="291">Asia/Rangoon</option>
                                        <option value="292">Asia/Riyadh</option>
                                        <option value="293">Asia/Saigon</option>
                                        <option value="294">Asia/Sakhalin</option>
                                        <option value="295">Asia/Samarkand</option>
                                        <option value="296">Asia/Seoul</option>
                                        <option value="297">Asia/Shanghai</option>
                                        <option value="298">Asia/Singapore</option>
                                        <option value="299">Asia/Taipei</option>
                                        <option value="300">Asia/Tashkent</option>
                                        <option value="301">Asia/Tbilisi</option>
                                        <option value="302">Asia/Tehran</option>
                                        <option value="303">Asia/Tel_Aviv</option>
                                        <option value="304">Asia/Thimbu</option>
                                        <option value="305">Asia/Thimphu</option>
                                        <option value="306">Asia/Tokyo</option>
                                        <option value="307">Asia/Ujung_Pandang</option>
                                        <option value="308">Asia/Ulaanbaatar</option>
                                        <option value="309">Asia/Ulan_Bator</option>
                                        <option value="310">Asia/Urumqi</option>
                                        <option value="311">Asia/Vientiane</option>
                                        <option value="312">Asia/Vladivostok</option>
                                        <option value="313">Asia/Yakutsk</option>
                                        <option value="314">Asia/Yekaterinburg</option>
                                        <option value="315">Asia/Yerevan</option>
                                        <option value="316">Atlantic/Azores</option>
                                        <option value="317">Atlantic/Bermuda</option>
                                        <option value="318">Atlantic/Canary</option>
                                        <option value="319">Atlantic/Cape_Verde</option>
                                        <option value="320">Atlantic/Faeroe</option>
                                        <option value="321">Atlantic/Faroe</option>
                                        <option value="322">Atlantic/Jan_Mayen</option>
                                        <option value="323">Atlantic/Madeira</option>
                                        <option value="324">Atlantic/Reykjavik</option>
                                        <option value="325">Atlantic/South_Georgia</option>
                                        <option value="326">Atlantic/Stanley</option>
                                        <option value="327">Atlantic/St_Helena</option>
                                        <option value="328">Australia/ACT</option>
                                        <option value="329">Australia/Adelaide</option>
                                        <option value="330">Australia/Brisbane</option>
                                        <option value="331">Australia/Broken_Hill</option>
                                        <option value="332">Australia/Canberra</option>
                                        <option value="333">Australia/Currie</option>
                                        <option value="334">Australia/Darwin</option>
                                        <option value="335">Australia/Eucla</option>
                                        <option value="336">Australia/Hobart</option>
                                        <option value="337">Australia/LHI</option>
                                        <option value="338">Australia/Lindeman</option>
                                        <option value="339">Australia/Lord_Howe</option>
                                        <option value="340">Australia/Melbourne</option>
                                        <option value="341">Australia/North</option>
                                        <option value="342">Australia/NSW</option>
                                        <option value="343">Australia/Perth</option>
                                        <option value="344">Australia/Queensland</option>
                                        <option value="345">Australia/South</option>
                                        <option value="346">Australia/Sydney</option>
                                        <option value="347">Australia/Tasmania</option>
                                        <option value="348">Australia/Victoria</option>
                                        <option value="349">Australia/West</option>
                                        <option value="350">Australia/Yancowinna</option>
                                        <option value="351">Europe/Amsterdam</option>
                                        <option value="352">Europe/Andorra</option>
                                        <option value="353">Europe/Athens</option>
                                        <option value="354">Europe/Belfast</option>
                                        <option value="355">Europe/Belgrade</option>
                                        <option value="356">Europe/Berlin</option>
                                        <option value="357">Europe/Bratislava</option>
                                        <option value="358">Europe/Brussels</option>
                                        <option value="359">Europe/Bucharest</option>
                                        <option value="360">Europe/Budapest</option>
                                        <option value="361">Europe/Chisinau</option>
                                        <option value="362">Europe/Copenhagen</option>
                                        <option value="363">Europe/Dublin</option>
                                        <option value="364">Europe/Gibraltar</option>
                                        <option value="365">Europe/Guernsey</option>
                                        <option value="366">Europe/Helsinki</option>
                                        <option value="367">Europe/Isle_of_Man</option>
                                        <option value="368">Europe/Istanbul</option>
                                        <option value="369">Europe/Jersey</option>
                                        <option value="370">Europe/Kaliningrad</option>
                                        <option value="371">Europe/Kiev</option>
                                        <option value="372">Europe/Lisbon</option>
                                        <option value="373">Europe/Ljubljana</option>
                                        <option value="374">Europe/London</option>
                                        <option value="375">Europe/Luxembourg</option>
                                        <option value="376">Europe/Madrid</option>
                                        <option value="377">Europe/Malta</option>
                                        <option value="378">Europe/Mariehamn</option>
                                        <option value="379">Europe/Minsk</option>
                                        <option value="380">Europe/Monaco</option>
                                        <option value="381">Europe/Moscow</option>
                                        <option value="382">Europe/Nicosia</option>
                                        <option value="383">Europe/Oslo</option>
                                        <option value="384">Europe/Paris</option>
                                        <option value="385">Europe/Podgorica</option>
                                        <option value="386">Europe/Prague</option>
                                        <option value="387">Europe/Riga</option>
                                        <option value="388">Europe/Rome</option>
                                        <option value="389">Europe/Samara</option>
                                        <option value="390">Europe/San_Marino</option>
                                        <option value="391">Europe/Sarajevo</option>
                                        <option value="392">Europe/Simferopol</option>
                                        <option value="393">Europe/Skopje</option>
                                        <option value="394">Europe/Sofia</option>
                                        <option value="395">Europe/Stockholm</option>
                                        <option value="396">Europe/Tallinn</option>
                                        <option value="397">Europe/Tirane</option>
                                        <option value="398">Europe/Tiraspol</option>
                                        <option value="399">Europe/Uzhgorod</option>
                                        <option value="400">Europe/Vaduz</option>
                                        <option value="401">Europe/Vatican</option>
                                        <option value="402">Europe/Vienna</option>
                                        <option value="403">Europe/Vilnius</option>
                                        <option value="404">Europe/Volgograd</option>
                                        <option value="405">Europe/Warsaw</option>
                                        <option value="406">Europe/Zagreb</option>
                                        <option value="407">Europe/Zaporozhye</option>
                                        <option value="408">Europe/Zurich</option>
                                        <option value="409">Indian/Antananarivo</option>
                                        <option value="410">Indian/Chagos</option>
                                        <option value="411">Indian/Christmas</option>
                                        <option value="412">Indian/Cocos</option>
                                        <option value="413">Indian/Comoro</option>
                                        <option value="414">Indian/Kerguelen</option>
                                        <option value="415">Indian/Mahe</option>
                                        <option value="416">Indian/Maldives</option>
                                        <option value="417">Indian/Mauritius</option>
                                        <option value="418">Indian/Mayotte</option>
                                        <option value="419">Indian/Reunion</option>
                                        <option value="420">Pacific/Apia</option>
                                        <option value="421">Pacific/Auckland</option>
                                        <option value="422">Pacific/Chatham</option>
                                        <option value="423">Pacific/Chuuk</option>
                                        <option value="424">Pacific/Easter</option>
                                        <option value="425">Pacific/Efate</option>
                                        <option value="426">Pacific/Enderbury</option>
                                        <option value="427">Pacific/Fakaofo</option>
                                        <option value="428">Pacific/Fiji</option>
                                        <option value="429">Pacific/Funafuti</option>
                                        <option value="430">Pacific/Galapagos</option>
                                        <option value="431">Pacific/Gambier</option>
                                        <option value="432">Pacific/Guadalcanal</option>
                                        <option value="433">Pacific/Guam</option>
                                        <option value="434">Pacific/Honolulu</option>
                                        <option value="435">Pacific/Johnston</option>
                                        <option value="436">Pacific/Kiritimati</option>
                                        <option value="437">Pacific/Kosrae</option>
                                        <option value="438">Pacific/Kwajalein</option>
                                        <option value="439">Pacific/Majuro</option>
                                        <option value="440">Pacific/Marquesas</option>
                                        <option value="441">Pacific/Midway</option>
                                        <option value="442">Pacific/Nauru</option>
                                        <option value="443">Pacific/Niue</option>
                                        <option value="444">Pacific/Norfolk</option>
                                        <option value="445">Pacific/Noumea</option>
                                        <option value="446">Pacific/Pago_Pago</option>
                                        <option value="447">Pacific/Palau</option>
                                        <option value="448">Pacific/Pitcairn</option>
                                        <option value="449">Pacific/Pohnpei</option>
                                        <option value="450">Pacific/Ponape</option>
                                        <option value="451">Pacific/Port_Moresby</option>
                                        <option value="452">Pacific/Rarotonga</option>
                                        <option value="453">Pacific/Saipan</option>
                                        <option value="454">Pacific/Samoa</option>
                                        <option value="455">Pacific/Tahiti</option>
                                        <option value="456">Pacific/Tarawa</option>
                                        <option value="457">Pacific/Tongatapu</option>
                                        <option value="458">Pacific/Truk</option>
                                        <option value="459">Pacific/Wake</option>
                                        <option value="460">Pacific/Wallis</option>
                                        <option value="461">Pacific/Yap</option>
                                    </select>
                                </td>


                            </tr>




                        </tbody>
                    </table>
                </div>
            </div>
            <!-- End Other Settings -->

            <!-- Online admission number start management -->
            <div class="yellow_box_notice">
                Note : Admission number cannot be changed once admissions have started
            </div>
            <div class="formCon_os">
                <div class="formConInner">
                    <h3>Online Admission Number Setup</h3>

                    <table style="width:100%" border="0" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td>
                                    <label>Admission Number Start <span class="required">*</span></label>                                    
                                    <asp:TextBox ID="tbx_online_admission_no_start" Enabled="false" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                    <div id="start_no_error" style="color: #F00"></div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div>

                
                   <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <button id="btn_apply" runat="server" onserverclick="btn_apply_ServerClick" style="width:150px" class="a_tag-btn"><span>Apply</span></button>
                    </li>
                </ul>
            </div>
        </div>

            </div>

                 </div>



        </div>
    </div>

    <script type="text/javascript">
        $('#submit_button_form').click(function (ev) {
            var start_no = $('#online_admission_no_start').val();
            if (start_no == '') {
                $('#start_no_error').html('Cannot be blank');
                return false;
            }
            var patt = /^\d*$/;
            var res = patt.test(start_no);
            if (!res) {
                $('#start_no_error').html('Allow Integers Only');
                return false;
            }

        });

        function validate(file) {

            var ext = file.split(".");
            ext = ext[ext.length - 1].toLowerCase();
            //alert(ext);
            var arrayExtensions = ["ico", "jpg"];

            if (arrayExtensions.lastIndexOf(ext) == -1) {                
                $('#' + '<%= lbl_info.ClientID %>').text("Wrong Extension Type.")
                $('.bs-info-modal-sm').modal('show');
                $('#' + '<%= fl_icon.ClientID %>').val("");
            }
        }
    </script>

     <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>

    <script type="text/javascript">
        $(function () {
            $('#' + '<%= tbx_founded.ClientID %>').datepicker({ autoclose: true });            
        });
    </script>

       <div class="modal fade bs-info-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" runat="server" id="lbl_info">Information</h4>
                </div>               
            </div>
        </div>
    </div>
</asp:Content>

