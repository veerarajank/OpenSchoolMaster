using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Text;
using System.IO;
using System.Linq;



/// <summary>
/// Summary description for Invoice_Pdf
/// </summary>
public class Invoice_Pdf
{
    public Invoice_Pdf()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public Invoice_Pdf(string saleid, string strticketno, string printmode)
    {

        string discount_print_continuetext = @"<tr>
                    <td style='vertical-align: central; text-align: left; width: 7%' colspan='7'>
                        <div style='text-align: right'><b>Continue...</b></div>
                    </td>
                </tr>";

        string withdiscount_print_continuetext = @"<tr>
                    <td style='vertical-align: central; text-align: left; width: 7%' colspan='7'>
                        <div style='text-align: right'><b>Continue.......</b></div>
                    </td>
                </tr>";




        string sale_customername = "Unknown";
        string sale_quoteno = "";
        string sale_quotedate = "";
        string deliverymode = "";
        string dispatchdate = "";
        string buyer_tinno = "";

        string str_salesdetailqry = @"select 
                                        sale_id,sale_customername,
                                        dbo.UDF_DateFormat(sale_date,1,1) as sale_date,
                                        dbo.UDF_DateFormat(sale_date,1,0) as dispatchdate,
                                        isnull(sale_billdiscount,0) as sale_billdiscount,
                                        isnull(Sale_shipmentcharge,0) as Sale_shipmentcharge,
                                        case when ISNULL(sale_billmethod,'0')='1' then 'INVOICE' else 'ESTIMATE' end as sale_billmethod,
                                        case when ISNULL(sale_billmethod,'0')='1' then ISNULL(Inv_billNo,'') 
	                                        when ISNULL(sale_billmethod,'0')='2' then ISNULL(Est_billNo,'') 
	                                        else '' end as billNo,
                                        financialyear,sale_custid,
                                        case when ISNULL(deliverymode,'')='' then 'Hand Delivery' 	                                        
	                                        else ISNULL(deliverymode,'Hand Delivery') end as deliverymode,



                                        case when ISNULL(sale_quoteno,'Verbal')='' then 'Verbal' else  ISNULL(sale_quoteno,'Verbal') end  as Verbal, 
                                        case when dbo.UDF_DateFormat(ISNULL(sale_quotedate,'1900-01-01'),1,0)='01-01-1900'  then dbo.UDF_DateFormat(sale_date,1,0) else dbo.UDF_DateFormat(sale_quotedate,1,0) end  as sale_quotedate
                                        ,isnull(orphan_id,'0') as orphan_id
                                         from sandd_tbl_countersale where sale_id=@sale_id";

        string billmethod = "";
        string sale_custid = "";
        string orphan_custid = "";
        double discount = 0.0;
        double Sale_shipmentcharge = 0.0;
        string strDate = "";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@sale_id", Convert.ToString(saleid).Trim());
        DataTable dtsale = new DataTable();
        dtsale = ies.Fn_ExecutiveSql_Datatable(str_salesdetailqry, 0, 1);

        if (dtsale != null)
        {
            if (dtsale.Rows.Count > 0)
            {
                strticketno = Convert.ToString(dtsale.Rows[0]["billNo"]);
                billmethod = Convert.ToString(dtsale.Rows[0]["sale_billmethod"]);
                discount = Convert.ToDouble(Convert.ToString(dtsale.Rows[0]["sale_billdiscount"]));
                Sale_shipmentcharge = Convert.ToDouble(Convert.ToString(dtsale.Rows[0]["Sale_shipmentcharge"]));
                strDate = Convert.ToString(dtsale.Rows[0]["sale_date"]);
                sale_customername = Convert.ToString(dtsale.Rows[0]["sale_customername"]);

                sale_custid = Convert.ToString(dtsale.Rows[0]["sale_custid"]);
                orphan_custid = Convert.ToString(dtsale.Rows[0]["orphan_id"]);
                sale_quoteno = Convert.ToString(dtsale.Rows[0]["Verbal"]);
                sale_quotedate = Convert.ToString(dtsale.Rows[0]["sale_quotedate"]);
                deliverymode = Convert.ToString(dtsale.Rows[0]["deliverymode"]);
                dispatchdate = Convert.ToString(dtsale.Rows[0]["dispatchdate"]);
            }
        }



        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        string strquery = @"select top 1 * from (select distinct om.CompanyName as Company,cm.Mobile,cm.emailid,om.Bill_Area as Area,Bill_District as District,om.Bill_Pincode as Pincode,cm.company_id,om.Bill_Address as Address,
                                    Bill_State as State,companyno,ISNULL(GSTIN,'') as GSTIN
                                    from tbl_customercontact cm inner join tbl_customer om on om.company_id=cm.company_id ) T where 1=1";
        strquery += " and company_id='" + sale_custid + "'";
        DataTable dt_companycontact = ies.Fn_ExecutiveSql_Datatable(strquery, 0, 1);


        string str_qry =
        @"SELECT
                ROW_NUMBER() over(order by sale_itemid) as slno,
                case when ISNULL(hsn_code,'')='' then '' else  ISNULL(hsn_code,'')  end  as HSN,
                case 
                when sale_producttype=1 then
                (
                select 
               spare_name + ' (' + spare_code + ')'    
                from tbl_sparedetails pt 
                 where spareid=sale_productcode)
                when sale_producttype=2 then
                (select isnull(s.product_name,'NA') + ' (' + isnull(s.product_code,'NA') + ')' from tbl_productdetails s where pid=sale_productcode)
                when sale_producttype=3 then
                it.ProductName
                end as Name,  
                isnull(Specification,'') as Specification,
                isnull(SpecificationInfo,'')  as SpecificationInfo,                
                sale_rate,
                sale_qty,
                convert(float,((sale_rate * sale_qty))) as total,
                sale_discount as sale_discount,
                ((convert(float,it.sale_rate) * convert(float,it.sale_qty)) - isnull(it.sale_discount,0)) as withouttaxamt,
                ((convert(float,it.sale_rate) * convert(float,it.sale_qty)) - isnull(it.sale_discount,0))
                    + 
                isnull((select ((convert(float,it.sale_rate) * convert(float,it.sale_qty)) - isnull(it.sale_discount,0))
                * (SUM(convert(float,sale_taxvalue))/100) FROM sandd_tbl_countersale_items_tax tx where tx.sale_id=it.sale_id and tx.sale_itemid=it.sale_itemid),0) as totalrate,
                isnull((select ((convert(float,it.sale_rate) * convert(float,it.sale_qty)) - isnull(it.sale_discount,0))
                * (SUM(convert(float,sale_taxvalue))/100) FROM sandd_tbl_countersale_items_tax tx where tx.sale_id=it.sale_id and tx.sale_itemid=it.sale_itemid),0) as taxrate,
                isnull(STUFF((SELECT ',' + (select sub.code from tbl_taxmaster sub where  sub.id=tx.sale_tax)   + '@ ' + CONVERT(varchar,sale_taxvalue) + '%' FROM sandd_tbl_countersale_items_tax tx where tx.sale_id=it.sale_id and tx.sale_itemid=it.sale_itemid                  
                FOR XML PATH (''), TYPE).value('.', 'varchar(max)'), 1, 1, ''),'') as taxname
                  FROM sandd_tbl_countersale_items it where sale_id=@sale_id";

        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@sale_id", Convert.ToString(saleid).Trim());
        DataTable dtitem = new DataTable();
        dtitem = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);

        var discountcount = dtitem.AsEnumerable().
        Count(row => row.Field<double>("sale_discount") != 0.0);

        string strbcData = strticketno;




        List<string> filelist = new List<string>();
        int totalfile = 0;
        string filefolder = HttpContext.Current.Server.MapPath("~/Invoice");
        if (System.IO.Directory.Exists(filefolder) == true)
        {
            DirectoryInfo dr = new DirectoryInfo(filefolder);
            foreach (var fi in dr.GetFiles())
            {
                File.Delete(fi.FullName);
            }
        }
        else
        {
            System.IO.Directory.CreateDirectory(filefolder);
        }



        string Rootpath = HttpContext.Current.Server.MapPath("~");

        string strpath_withdiscount = Rootpath + @"\Format\invoice_finalwithdiscount.html";
        string strpath_withoutdiscount = Rootpath + @"\Format\invoice_finalwithoutdiscount.html";

        string filecontent;


        //Print buying goods
        double total = 0.0;

        double taxamt = 0.0;
        double itemdiscount = 0.0;

        Int32 totalqty = 0;
        int i = 0;
        int currow = 0;


        double sale_Billamt = 0;
        double receivedamt = 0;
        string Sales_paymentmode = "";


        string str_payment = @"SELECT isnull(sale_Billamt,0) as sale_Billamt,isnull(receivedamt,0) as receivedamt,isnull(Sales_paymentmode,'') as Sales_paymentmode  FROM sandd_tbl_counterpayment where sale_id=@sale_id";
        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@sale_id", Convert.ToString(saleid).Trim());
        dtsale = new DataTable();
        dtsale = ies.Fn_ExecutiveSql_Datatable(str_payment, 0, 1);

        if (dtsale != null)
        {
            if (dtsale.Rows.Count > 0)
            {
                sale_Billamt = Convert.ToDouble(dtsale.Rows[0]["sale_Billamt"]);
                receivedamt = Convert.ToDouble(dtsale.Rows[0]["receivedamt"]);
                Sales_paymentmode = Convert.ToString(dtsale.Rows[0]["Sales_paymentmode"]);
            }
        }





    ContinueFinal:

        string filepath = filefolder + @"\" + "File_" + Convert.ToString(totalfile) + ".html";
        if (File.Exists(filepath))
        {
            File.Delete(filepath);
        }
        StreamWriter writer = new StreamWriter(filepath);
        filelist.Add(filepath);

        try
        {


            bool isdiscount = false;

            string continuetext = "";

            if (discountcount != 0)
            {
                var fileStream = new FileStream(strpath_withdiscount, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    filecontent = streamReader.ReadToEnd();
                }

                isdiscount = true;
                continuetext = discount_print_continuetext;
            }
            else
            {
                var fileStream = new FileStream(strpath_withoutdiscount, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    filecontent = streamReader.ReadToEnd();
                }
                continuetext = withdiscount_print_continuetext;
            }





            if (dt_companycontact != null)
            {
                if (dt_companycontact.Rows.Count > 0)
                {

                    
                    string buyer_GSTIN = Convert.ToString(dt_companycontact.Rows[0]["GSTIN"]);
                    filecontent = filecontent.Replace("{{companyname}}", Convert.ToString(dt_companycontact.Rows[0]["Company"]));


                    filecontent = filecontent.Replace("{{area}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));

                    filecontent = filecontent.Replace("{{state}}", Convert.ToString(dt_companycontact.Rows[0]["District"]) + "," + Convert.ToString(dt_companycontact.Rows[0]["state"]));
                    if (buyer_tinno != "" && buyer_GSTIN != "")
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".<br>" + "Buyer's Sales Tax No." + buyer_tinno + "<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                    }
                    else if (buyer_tinno == "" && buyer_GSTIN != "")
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                    }
                    else
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".");
                    }

                    filecontent = filecontent.Replace("{{address}}", Convert.ToString(dt_companycontact.Rows[0]["address"]));

                    filecontent = filecontent.Replace("{{customerrefno}}", Convert.ToString(dt_companycontact.Rows[0]["companyno"]));

                    filecontent = filecontent.Replace("{{otherref}}", "-");
                    filecontent = filecontent.Replace("{{deliverynote}}", "-");
                    filecontent = filecontent.Replace("{{dispatchto}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));



                }
               
            }
            else
            {
                filecontent = filecontent.Replace("{{companyname}}", sale_customername);

                filecontent = filecontent.Replace("<div>{{area}},</div>", "");
                filecontent = filecontent.Replace("<div>{{state}} - {{postal}}.</div>", "");
                filecontent = filecontent.Replace("<div>{{address}},</div>", "");


                filecontent = filecontent.Replace("{{postal}}", "&nbsp;");
                filecontent = filecontent.Replace("<div>{{area}},</div>", "");
                filecontent = filecontent.Replace("{{area}}", "&nbsp;");
                filecontent = filecontent.Replace("{{state}}", "&nbsp;");
                filecontent = filecontent.Replace("<div>{{address}},</div>", "");
                filecontent = filecontent.Replace("{{address}}", "&nbsp;");
                filecontent = filecontent.Replace("{{customerrefno}}", "-");
                filecontent = filecontent.Replace("{{otherref}}", "-");
                filecontent = filecontent.Replace("{{deliverynote}}", "-");
                filecontent = filecontent.Replace("{{dispatchto}}", "-");
            }


            filecontent = filecontent.Replace("{{ORIGINAL}}", printmode);
            filecontent = filecontent.Replace("{{invoiceno}}", strticketno);
            filecontent = filecontent.Replace("{{invoiceno}}", strticketno);
            filecontent = filecontent.Replace("{{invoicedate}}", strDate);
            filecontent = filecontent.Replace("{{deliverynote}}", "-");
            filecontent = filecontent.Replace("{{buyerorderno}}", sale_quoteno);
            filecontent = filecontent.Replace("{{buyerordernodate}}", sale_quotedate);
            filecontent = filecontent.Replace("{{dispatchdocno}}", "-");
            filecontent = filecontent.Replace("{{dispatchthrough}}", deliverymode);
            filecontent = filecontent.Replace("{{dispatchdate}}", dispatchdate);

            filecontent = filecontent.Replace("{{modeofpayment}}", Sales_paymentmode);


            int initialrow = 0;

            string item_details = string.Empty;

            if (dtitem != null)
            {


                for (i = currow; i < dtitem.Rows.Count; i++)
                {

                    int checkrow = 15;

                    if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                    {
                        checkrow = 12;
                    }

                    initialrow += 1;
                    currow = i;
                    string verticalcenter = "center";
                    string str_trstyle = " style='vertical-align:" + verticalcenter + ";height:30px'";
                    if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                    {
                        str_trstyle = " style='vertical-align: top;height:60px'";
                        verticalcenter = "top";
                    }
                    if (dtitem.Rows.Count - 1 == i)
                    {
                        int totallength = 15 - initialrow;

                        if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                        {
                            totallength = 12 - initialrow;
                        }


                        if (totallength > 0)
                        {
                            verticalcenter = "top";
                            string t_length = (totallength * 25).ToString() + "px";
                            str_trstyle = " style='vertical-align:" + verticalcenter + ";height:" + t_length + @"'";

                            if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                            {
                                //str_trstyle = " style='vertical-align: " + verticalcenter + ";height:60px'";
                            }
                        }
                    }

                    string description = Convert.ToString(dtitem.Rows[i]["Name"]);
                    string HSN = Convert.ToString(dtitem.Rows[i]["HSN"]);
                    string Specification = Convert.ToString(dtitem.Rows[i]["Specification"]).Trim();
                    string SpecificationInfo = Convert.ToString(dtitem.Rows[i]["SpecificationInfo"]).Trim();
                    if (Specification != "")
                    {
                        description += "<br /><b>" + Specification + "</b>";
                    }
                    if (SpecificationInfo != "")
                    {
                        description += "<br /><b>" + SpecificationInfo + "</b>";
                    }

                    string sale_qty = Convert.ToString(dtitem.Rows[i]["sale_qty"]);
                    string sale_rate = Convert.ToDouble(dtitem.Rows[i]["sale_rate"]).ToString("F");
                    string sale_discount = Convert.ToDouble(dtitem.Rows[i]["sale_discount"]).ToString("F");
                    string withouttaxamt = Convert.ToDouble(dtitem.Rows[i]["withouttaxamt"]).ToString("F");
                    string taxname = Convert.ToString(dtitem.Rows[i]["taxname"]);
                    string taxrate = Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["taxrate"])).ToString("F");

                    if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                    {
                        description += "<br /><div style='text-align:right!important'>" + taxname + "</div>";
                        sale_rate += "<br /><div style='text-align:right!important'>&nbsp;</div>";
                        withouttaxamt += "<br /><div style='text-align:right!important'>" + taxrate + "</div>";
                    }

                    if (isdiscount == true)
                    {
                        if (item_details == string.Empty)
                        {

                            item_details = @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align: " + verticalcenter + ";text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='2' style='vertical-align:" + verticalcenter + ";'>" + description + @"</td>                    
                                                         <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                            
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_discount + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                        }
                        else
                        {
                            item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='2' style='vertical-align:" + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                            
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_discount + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                        }
                    }
                    else
                    {
                        if (item_details == string.Empty)
                        {

                            item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                                                                                    
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                        }
                        else
                        {
                            item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                                                                                    
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                        }
                    }

                    total += Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["withouttaxamt"]));
                    totalqty += Convert.ToInt32(Convert.ToString(dtitem.Rows[i]["sale_qty"]));
                    taxamt += Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["taxrate"]));
                    itemdiscount += Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["sale_discount"]));

                    if (initialrow == checkrow)
                    {
                        filecontent = filecontent.Replace("{{rowdata}}", item_details);
                        filecontent = filecontent.Replace("{{modeofpayment}}", Sales_paymentmode);





                        writer.Write(filecontent);
                        writer.Write(continuetext);

                        writer.Write("</table>This is a Computer Generated Invoice</div></div></body></html>");
                        writer.Close();
                        totalfile += 1;
                        initialrow = 0;
                        currow += 1;
                        goto ContinueFinal;
                    }

                }
            }



            filecontent = filecontent.Replace("{{rowdata}}", item_details);

            writer.Write(filecontent);
            filecontent = "";

            strpath_withdiscount = Rootpath + @"\Format\invoice_discount.html";
            strpath_withoutdiscount = Rootpath + @"\Format\invoice_withoutdiscount.html";



            isdiscount = false;



            if (discountcount != 0)
            {
                var fileStream = new FileStream(strpath_withdiscount, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    filecontent = streamReader.ReadToEnd();
                }

                isdiscount = true;

            }
            else
            {
                var fileStream = new FileStream(strpath_withoutdiscount, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    filecontent = streamReader.ReadToEnd();
                }

            }




            filecontent = filecontent.Replace("{{totalrate}}", "&nbsp;");
            filecontent = filecontent.Replace("{{totalqty}}", totalqty.ToString());
            filecontent = filecontent.Replace("{{totaldiscount}}", Convert.ToDouble(itemdiscount).ToString("F"));


            string consolidatetotal = "";
            string colscnt = "7";

            consolidatetotal = (total + taxamt).ToString();
            filecontent = filecontent.Replace("{{totalamount}}", consolidatetotal);
            double totalamt = (total + taxamt) - discount;
            string consolidatedtax = "";


            string str_consolidatetaxqry = @"select                                                 
                                                            isnull(STUFF((SELECT ',' + (select sub.code from tbl_taxmaster sub where  sub.id=tx.sale_tax)  + '@' + CONVERT(varchar,sale_taxvalue) + '%' FROM sandd_tbl_countersale_tax tx where tx.sale_id=it.sale_id and ISNULL(sale_taxvalue,0)!=0 
                                                            FOR XML PATH (''), TYPE).value('.', 'varchar(max)'), 1, 1, ''),'') as taxname,
                                                            SUM(sale_taxvalue)   as sale_taxvalue                                              
                                                              FROM sandd_tbl_countersale_tax it where ISNULL(sale_taxvalue,0)!=0 and sale_id=@sale_id
                                                              group by sale_id";

            str_consolidatetaxqry = @"select(select sub.code from tbl_taxmaster sub where sub.id=it.sale_tax)  + '@' +  CONVERT(varchar,sale_taxvalue)  as taxname,
                                    sale_taxvalue   as sale_taxvalue   FROM sandd_tbl_countersale_tax it where ISNULL(sale_taxvalue,0)!=0 and it.sale_id=@sale_id";

            string con_taxname = "";
            double con_taxvalue = 0;
            ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@sale_id", Convert.ToString(saleid).Trim());
            dtsale = new DataTable();
            dtsale = ies.Fn_ExecutiveSql_Datatable(str_consolidatetaxqry, 0, 1);

            if (dtsale != null)
            {
                if (dtsale.Rows.Count > 0)
                {

                    for (int itaxid = 0; itaxid < dtsale.Rows.Count; itaxid++)
                    {

                        con_taxname = Convert.ToString(dtsale.Rows[itaxid]["taxname"]);
                        con_taxvalue = Convert.ToDouble(dtsale.Rows[itaxid]["sale_taxvalue"]);

                        if (con_taxname != "")
                        {
                            double totalconsolidatetaxvalue = 0;
                            if (con_taxvalue != 0)
                            {
                                totalconsolidatetaxvalue = (Convert.ToDouble(total) - Convert.ToDouble(discount)) * Convert.ToDouble(Convert.ToDouble(con_taxvalue) / Convert.ToDouble(100));
                                totalamt = totalamt + totalconsolidatetaxvalue;

                            }

                            consolidatedtax += @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>" + con_taxname + @"%&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (totalconsolidatetaxvalue).ToString("F") + "</td></tr>";


                        }

                    }
                }
            }





            filecontent = filecontent.Replace("{{consolidatetax}}", consolidatedtax);

            string consolidatediscount = "";



            if (discount != 0)
            {

                consolidatediscount = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Discount&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (discount).ToString("F") + "</td></tr>";
            }

            filecontent = filecontent.Replace("{{overalldiscount}}", consolidatediscount);












            string roundoff = roundoff = (totalamt - Math.Round(totalamt, 0)).ToString("0.00");

            if (roundoff.IndexOf("-") == 0) roundoff = roundoff.Replace('-', '+');
            else roundoff = "-" + roundoff;

            string consolidateroundoff = "";
            if (roundoff != "" && roundoff != "-0.00" && roundoff != "0.00" && roundoff != "+0.00")
            {

                consolidateroundoff = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Rounded Off&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (roundoff) + "</td></tr>";
            }


            filecontent = filecontent.Replace("{{overallroundoff}}", consolidateroundoff);



            string consolidatenettotal = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Net Total&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (Math.Round(totalamt, 0)).ToString("F") + "</td></tr>";


            filecontent = filecontent.Replace("{{nettotal}}", consolidatenettotal);







            string consolidateShipmentCharge = "";


            if (Sale_shipmentcharge != 0)
            {

                consolidateShipmentCharge = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Shipment Charge&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (Sale_shipmentcharge).ToString("F") + "</td></tr>";

            }

            filecontent = filecontent.Replace("{{shipmentcharge}}", consolidateShipmentCharge);

            double grandamt = 0;


            grandamt = totalamt + Sale_shipmentcharge;

            string consolidategrandtotal = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Grand Total&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (Math.Round(grandamt, 0)).ToString("F") + "</td></tr>";


            filecontent = filecontent.Replace("{{grandtotal}}", consolidategrandtotal);




            string amtwords;
            string totalbillamt = (Math.Round(grandamt, 0)).ToString("F");
            IndiaCurrencyConverter con = new IndiaCurrencyConverter();
            if (IsNumeric(totalbillamt))
            {
                amtwords =
                (con.ConvertToWord(totalbillamt, System.Drawing.Color.Blue, "0") + " rupees only");
            }
            else
            {
                string amt = totalbillamt.ToString().Split('.')[0];
                string paise = Convert.ToDouble(totalbillamt).ToString("0.00").Split('.')[1];

                string totalamtinwords = "";
                string totalpaiseinwords = "";
                if (Convert.ToDouble(paise) == 0)
                {
                    totalamtinwords = con.ConvertToWord(amt, System.Drawing.Color.Blue, "1") + " rupees only";
                }
                else
                {
                    totalamtinwords = con.ConvertToWord(amt, System.Drawing.Color.Blue, "1") + " rupees and ";
                    totalpaiseinwords = con.ConvertToWord(paise, System.Drawing.Color.Blue, "1") + " paise only";
                }


                amtwords = (totalamtinwords + totalpaiseinwords);
            }

            filecontent = filecontent.Replace("{{amountinwords}}", amtwords);




            if (billmethod == "INVOICE")
            {


                str_payment = @"SELECT isnull(sale_Billamt,0) as sale_Billamt,isnull(receivedamt,0) as receivedamt,isnull(Sales_paymentmode,'') as Sales_paymentmode  FROM sandd_tbl_counterpayment where sale_id=@sale_id";



                ies = new IES_Generic_Function();
                ies.ies_parameters.Clear();
                ies.ies_parameters.Add("@sale_id", Convert.ToString(saleid).Trim());
                dtsale = new DataTable();
                dtsale = ies.Fn_ExecutiveSql_Datatable(str_payment, 0, 1);

                if (dtsale != null)
                {
                    if (dtsale.Rows.Count > 0)
                    {
                        sale_Billamt = Convert.ToDouble(dtsale.Rows[0]["sale_Billamt"]);
                        receivedamt = Convert.ToDouble(dtsale.Rows[0]["receivedamt"]);
                        Sales_paymentmode = Convert.ToString(dtsale.Rows[0]["Sales_paymentmode"]);
                        filecontent = filecontent.Replace("{{modeofpayment}}", Sales_paymentmode);


                    }
                }
            }



            writer.Write(filecontent);
            writer.Write("</table>This is a Computer Generated Invoice</div></div></body></html>");
            writer.Close();
            //MessageBox.Show(filecontent);
            // web_report.DocumentText = filecontent;
            string strpath = Rootpath + "/Invoice";
            string domainname = "0001";
            string[] Inputfile = filelist.ToArray();
            PdfGenerator.Invoicepdf(strpath, domainname,
                Inputfile);

            //pdfViewer1.Document = new PdfDocument(@"D:\Chinna\IES\Development\Xsell_Retail\Xsell_Retail\Xsell_Retail\bin\Debug\001\0001.pdf");
            string file_pathfile = strpath + "/0001.pdf";
            //axAcroPDF1.LoadFile(file_pathfile);

        }
        catch
        {

        }

        // When a cursor is back to its default shape, it means the process ends

    }

    //-------------------------------Structures and classes----------------------------

    internal bool IsNumeric(string ValueInNumeric)
    {
        bool IsFine = true;
        foreach (char ch in ValueInNumeric)
        {
            if (!(ch >= '0' && ch <= '9'))
            {
                IsFine = false;
            }
        }
        return IsFine;
    }
}






///////////////////////



/// <summary>
/// Summary description for Invoice_Pdf
/// </summary>
public class DC_Pdf
{
    public DC_Pdf()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public DC_Pdf(string dc_id, string strticketno)
    {

        string discount_print_continuetext = @"<tr>
                    <td style='vertical-align: central; text-align: left; width: 7%' colspan='7'>
                        <div style='text-align: right'><b>Continue...</b></div>
                    </td>
                </tr>";

        string withdiscount_print_continuetext = @"<tr>
                    <td style='vertical-align: central; text-align: left; width: 7%' colspan='7'>
                        <div style='text-align: right'><b>Continue.......</b></div>
                    </td>
                </tr>";




        string sale_customername = "Unknown";
        string sale_quoteno = "";
        string sale_quotedate = "";
        string deliverymode = "";
        string dispatchdate = "";
        string buyer_tinno = "";

        string str_salesdetailqry = @"select 
                                        dc_id,dc_customername,
                                        dbo.UDF_DateFormat(dc_date,1,1) as dc_date,
                                        dbo.UDF_DateFormat(dc_date,1,0) as dispatchdate,
                                        isnull(dc_billdiscount,0) as dc_billdiscount,
                                        isnull(dc_shipmentcharge,0) as dc_shipmentcharge,                                        
                                        dc_ticketno as billNo,
                                        financialyear,dc_customerid,
                                        case when ISNULL(dc_deliverymode,'')='' then 'Hand Delivery' 	                                        
	                                        else ISNULL(dc_deliverymode,'Hand Delivery') end as deliverymode,
                                        case when ISNULL(dc_buyerorderno,'Verbal')='' then 'Verbal' else  ISNULL(dc_buyerorderno,'Verbal') end  as Verbal, 
                                        case when dbo.UDF_DateFormat(ISNULL(dc_buyerorderdate,'1900-01-01'),1,0)='01-01-1900'  then dbo.UDF_DateFormat(dc_date,1,0) else dbo.UDF_DateFormat(dc_buyerorderdate,1,0) end  as dc_quotedate
                                        ,isnull(orphan_id,'0') as orphan_id,isnull(dc_method,'1') as dc_method,isnull(point1,'') as point1,isnull(point2,'') as point2,isnull(point3,'') as point3
                                         from sandd_tbl_sales_dc where dc_id=@dc_id";

        string billmethod = "";
        string dc_method = "1";
        string Terms = "";
        string sale_custid = "";
        string orphan_custid = "";
        double discount = 0.0;
        double Sale_shipmentcharge = 0.0;
        string strDate = "";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@dc_id", Convert.ToString(dc_id).Trim());
        DataTable dtsale = new DataTable();
        dtsale = ies.Fn_ExecutiveSql_Datatable(str_salesdetailqry, 0, 1);

        if (dtsale != null)
        {
            if (dtsale.Rows.Count > 0)
            {
                strticketno = Convert.ToString(dtsale.Rows[0]["billNo"]);
                billmethod = Convert.ToString("Delivery Challan");
                dc_method = Convert.ToString(dtsale.Rows[0]["dc_method"]);
                discount = Convert.ToDouble(Convert.ToString(dtsale.Rows[0]["dc_billdiscount"]));
                Sale_shipmentcharge = Convert.ToDouble(Convert.ToString(dtsale.Rows[0]["dc_shipmentcharge"]));
                strDate = Convert.ToString(dtsale.Rows[0]["dc_date"]);
                sale_customername = Convert.ToString(dtsale.Rows[0]["dc_customername"]);
                Terms = "<ul>";
                if (Convert.ToString(dtsale.Rows[0]["point1"]).Trim() != "")
                {
                    Terms += "<li>" + Convert.ToString(dtsale.Rows[0]["point1"]) + "</li>";
                }
                if (Convert.ToString(dtsale.Rows[0]["point2"]).Trim() != "")
                {
                    Terms += "<li>" + Convert.ToString(dtsale.Rows[0]["point2"]) + "</li>";
                }
                if (Convert.ToString(dtsale.Rows[0]["point3"]).Trim() != "")
                {
                    Terms += "<li>" + Convert.ToString(dtsale.Rows[0]["point3"]) + "</li>";
                }
                Terms += "</ul>";

                sale_custid = Convert.ToString(dtsale.Rows[0]["dc_customerid"]);
                orphan_custid = Convert.ToString(dtsale.Rows[0]["orphan_id"]);
                sale_quoteno = Convert.ToString(dtsale.Rows[0]["Verbal"]);
                sale_quotedate = Convert.ToString(dtsale.Rows[0]["dc_quotedate"]);
                deliverymode = Convert.ToString(dtsale.Rows[0]["deliverymode"]);
                dispatchdate = Convert.ToString(dtsale.Rows[0]["dispatchdate"]);
            }
        }



        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        string strquery = @"select top 1 * from (select distinct om.CompanyName as Company,cm.Mobile,cm.emailid,om.Area,District as[District],om.Pincode,cm.company_id,om.Address,
                                    (select st.Name from tbl_statemaster st where om.State=st.id) as State,companyno,isnull(om.TinNo,'') as TinNo,ISNULL(GSTIN,'') as GSTIN
                                    from tbl_Companycustomercontact cm inner join tbl_companycustomer om on om.company_id=cm.company_id ) T where 1=1";
        strquery += " and company_id='" + sale_custid + "'";
        DataTable dt_companycontact = ies.Fn_ExecutiveSql_Datatable(strquery, 0, 1);


        string str_qry =
        @"			SELECT
                ROW_NUMBER() over(order by dc_itemid) as slno,
                case when ISNULL(hsn_code,'')='' then 
                
                case when dc_producttype=1 then (select isnull(pt.hsn_code,'') from store_component pt where Componentid=dc_productcode)
                when dc_producttype=2 then (select isnull(s.hsn_code,'')  from store_Product s where Product_id=dc_productcode)  else '' end

                else  ISNULL(hsn_code,'')  end  as HSN,
                case 
                when dc_producttype=1 then
                (
                select 
                isnull((select cn.ShortName from store_ComponentName cn where cn.ComponentID=pt.ComponentName),'NA')                                
                + '-' + isnull((select cn.ShortName from store_Packagetype cn where cn.PackageID=pt.Packagetype),'NA')    
                + '-' + isnull((select cn.PrimaryValue from store_PrimaryValue cn where cn.PrimaryID=pt.ComponentValue),'NA')     
                from store_component pt 
                 where Componentid=dc_productcode)
                when dc_producttype=2 then
                (select s.Name from store_Product s where Product_id=dc_productcode)                
                end as Name,
                dc_rate,
                dc_qty,
                convert(float,((dc_rate * dc_qty))) as total,
                dc_discount as dc_discount,
                ((convert(float,it.dc_rate) * convert(float,it.dc_qty)) - isnull(it.dc_discount,0)) as withouttaxamt,
                ((convert(float,it.dc_rate) * convert(float,it.dc_qty)) - isnull(it.dc_discount,0))
                    + 
                isnull((select ((convert(float,it.dc_rate) * convert(float,it.dc_qty)) - isnull(it.dc_discount,0))
                * (SUM(convert(float,dc_itemtaxvalue))/100) FROM sandd_tbl_sales_dc_items_tax tx where tx.dc_id=it.dc_id and tx.dc_itemid=it.dc_itemid),0) as totalrate,
                isnull((select ((convert(float,it.dc_rate) * convert(float,it.dc_qty)) - isnull(it.dc_discount,0))
                * (SUM(convert(float,tx.dc_itemtaxvalue))/100) FROM sandd_tbl_sales_dc_items_tax tx where tx.dc_id=it.dc_id and tx.dc_itemid=it.dc_itemid),0) as taxrate,
                isnull(STUFF((SELECT ',' + (select sub.code from tbl_taxmaster sub where  sub.id=tx.dc_itemtax)   + '@ ' + CONVERT(varchar,tx.dc_itemtaxvalue) + '%' FROM sandd_tbl_sales_dc_items_tax tx where tx.dc_id=it.dc_id and tx.dc_itemid=it.dc_itemid                  
                FOR XML PATH (''), TYPE).value('.', 'varchar(max)'), 1, 1, ''),'') as taxname,
                case when isnull(dc_serialno,'')='' then '' else isnull(dc_serialno,'') end as dc_serialno
                  FROM sandd_tbl_sales_dc_items it where dc_id=@dc_id";

        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@dc_id", Convert.ToString(dc_id).Trim());
        DataTable dtitem = new DataTable();
        dtitem = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);

        var discountcount = dtitem.AsEnumerable().
        Count(row => row.Field<double>("dc_discount") != 0.0);

        string strbcData = strticketno;




        List<string> filelist = new List<string>();
        int totalfile = 0;
        string filefolder = HttpContext.Current.Server.MapPath("~/Invoice");
        if (System.IO.Directory.Exists(filefolder) == true)
        {
            DirectoryInfo dr = new DirectoryInfo(filefolder);
            foreach (var fi in dr.GetFiles())
            {
                File.Delete(fi.FullName);
            }
        }
        else
        {
            System.IO.Directory.CreateDirectory(filefolder);
        }



        string Rootpath = HttpContext.Current.Server.MapPath("~");

        string strpath_withdiscount = Rootpath + @"\Format\dc_invoice_finalwithdiscount.html";
        string strpath_withoutdiscount = Rootpath + @"\Format\dc_invoice_finalwithoutdiscount.html";
        string strpath_withoutrate = Rootpath + @"\Format\dc_invoice_finalwithoutrate.html";

        string filecontent;


        //Print buying goods
        double total = 0.0;

        double taxamt = 0.0;
        double itemdiscount = 0.0;

        Int32 totalqty = 0;
        int i = 0;
        int currow = 0;


        double sale_Billamt = 0;
        double receivedamt = 0;
        string Sales_paymentmode = "";








    ContinueFinal:

        string filepath = filefolder + @"\" + "File_" + Convert.ToString(totalfile) + ".html";
        if (File.Exists(filepath))
        {
            File.Delete(filepath);
        }
        StreamWriter writer = new StreamWriter(filepath);
        filelist.Add(filepath);

        try
        {


            bool isdiscount = false;

            string continuetext = "";

            if (dc_method == "1")
            {

                if (discountcount != 0)
                {
                    var fileStream = new FileStream(strpath_withdiscount, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        filecontent = streamReader.ReadToEnd();
                    }

                    isdiscount = true;
                    continuetext = discount_print_continuetext;
                }
                else
                {
                    var fileStream = new FileStream(strpath_withoutdiscount, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        filecontent = streamReader.ReadToEnd();
                    }
                    continuetext = withdiscount_print_continuetext;
                }
            }
            else
            {
                var fileStream = new FileStream(strpath_withoutrate, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    filecontent = streamReader.ReadToEnd();
                }
                continuetext = discount_print_continuetext;
            }





            if (dt_companycontact != null)
            {
                if (dt_companycontact.Rows.Count > 0)
                {

                    buyer_tinno = Convert.ToString(dt_companycontact.Rows[0]["TinNo"]);
                    string buyer_GSTIN = Convert.ToString(dt_companycontact.Rows[0]["GSTIN"]);
                    filecontent = filecontent.Replace("{{companyname}}", Convert.ToString(dt_companycontact.Rows[0]["Company"]));


                    filecontent = filecontent.Replace("{{area}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));

                    filecontent = filecontent.Replace("{{state}}", Convert.ToString(dt_companycontact.Rows[0]["District"]) + "," + Convert.ToString(dt_companycontact.Rows[0]["state"]));
                    if (buyer_tinno != "" && buyer_GSTIN != "")
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".<br>" + "Buyer's Sales Tax No." + buyer_tinno + "<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                    }
                    else if (buyer_tinno == "" && buyer_GSTIN != "")
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                    }
                    else
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".");
                    }

                    filecontent = filecontent.Replace("{{address}}", Convert.ToString(dt_companycontact.Rows[0]["address"]));

                    filecontent = filecontent.Replace("{{customerrefno}}", Convert.ToString(dt_companycontact.Rows[0]["companyno"]));

                    filecontent = filecontent.Replace("{{otherref}}", "-");
                    filecontent = filecontent.Replace("{{deliverynote}}", "-");
                    filecontent = filecontent.Replace("{{dispatchto}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));



                }
                else
                {

                    ies = new IES_Generic_Function();
                    ies.ies_parameters.Clear();
                    strquery = @"select isnull(om.CompanyName,'') as Company,
                                    ISNULL(om.Mobileno,'') as Mobile,isnull(om.emailid,'') as emailid,
                                    isnull(om.Area,'') as Area,
                                    isnull(District,'') as[District],
                                    isnull(om.Pincode,'') as Pincode,'' as Address,
                                    '' as State,'' as companyno,
                                    isnull(om.TinNo,'') as TinNo,
                                    ISNULL(GSTIN,'') as GSTIN,
                                    '0' as company_id
                                    from tbl_orphancustomer om where 1=1 ";
                    strquery += " and company_id='" + orphan_custid + "'";
                    dt_companycontact = ies.Fn_ExecutiveSql_Datatable(strquery, 0, 1);


                    if (dt_companycontact != null)
                    {
                        if (dt_companycontact.Rows.Count > 0)
                        {

                            buyer_tinno = Convert.ToString(dt_companycontact.Rows[0]["TinNo"]);
                            string buyer_GSTIN = Convert.ToString(dt_companycontact.Rows[0]["GSTIN"]);
                            filecontent = filecontent.Replace("{{companyname}}", Convert.ToString(dt_companycontact.Rows[0]["Company"]));
                            if (Convert.ToString(dt_companycontact.Rows[0]["area"]) == "")
                            {
                                filecontent = filecontent.Replace("{{area}},", "");
                            }
                            else
                            {
                                filecontent = filecontent.Replace("{{area}},", Convert.ToString(dt_companycontact.Rows[0]["area"]) + ",");
                            }
                            if (Convert.ToString(dt_companycontact.Rows[0]["District"]) == "")
                            {
                                filecontent = filecontent.Replace("{{state}} - ", "");
                            }
                            else
                            {
                                filecontent = filecontent.Replace("{{state}} - ", Convert.ToString(dt_companycontact.Rows[0]["District"]) + ",");
                            }

                            if (buyer_tinno != "" && buyer_GSTIN != "")
                            {
                                filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + "<br>" + "Buyer's Sales Tax No." + buyer_tinno + "<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                            }
                            else if (buyer_tinno == "" && buyer_GSTIN != "")
                            {
                                filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + "<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                            }
                            else
                            {

                                if (Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) == "")
                                {
                                    filecontent = filecontent.Replace("{{postal}}", "");
                                }
                                else
                                {
                                    filecontent = filecontent.Replace("{{postal}}", "<br>" + Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".");
                                }

                            }
                            filecontent = filecontent.Replace("{{dispatchto}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));
                        }
                    }

                    filecontent = filecontent.Replace("{{companyname}}", sale_customername);
                    filecontent = filecontent.Replace("{{postal}}", "&nbsp;");
                    filecontent = filecontent.Replace("{{area}}", "&nbsp;");
                    filecontent = filecontent.Replace("{{area}},", "&nbsp;");
                    filecontent = filecontent.Replace("{{state}}", "&nbsp;");
                    filecontent = filecontent.Replace("<div>{{address}},</div>", "");
                    filecontent = filecontent.Replace("{{address}}", "&nbsp;");
                    filecontent = filecontent.Replace("{{address}},", "&nbsp;");
                    filecontent = filecontent.Replace("{{customerrefno}}", "-");
                    filecontent = filecontent.Replace("{{otherref}}", "-");
                    filecontent = filecontent.Replace("{{deliverynote}}", "-");
                    filecontent = filecontent.Replace("{{dispatchto}}", "-");
                }
            }
            else
            {
                filecontent = filecontent.Replace("{{companyname}}", sale_customername);

                filecontent = filecontent.Replace("<div>{{area}},</div>", "");
                filecontent = filecontent.Replace("<div>{{state}} - {{postal}}.</div>", "");
                filecontent = filecontent.Replace("<div>{{address}},</div>", "");


                filecontent = filecontent.Replace("{{postal}}", "&nbsp;");
                filecontent = filecontent.Replace("<div>{{area}},</div>", "");
                filecontent = filecontent.Replace("{{area}}", "&nbsp;");
                filecontent = filecontent.Replace("{{state}}", "&nbsp;");
                filecontent = filecontent.Replace("<div>{{address}},</div>", "");
                filecontent = filecontent.Replace("{{address}}", "&nbsp;");
                filecontent = filecontent.Replace("{{customerrefno}}", "-");
                filecontent = filecontent.Replace("{{otherref}}", "-");
                filecontent = filecontent.Replace("{{deliverynote}}", "-");
                filecontent = filecontent.Replace("{{dispatchto}}", "-");
            }


            filecontent = filecontent.Replace("{{invoiceno}}", strticketno);
            filecontent = filecontent.Replace("{{invoicedate}}", strDate);
            filecontent = filecontent.Replace("{{deliverynote}}", "-");
            filecontent = filecontent.Replace("{{buyerorderno}}", sale_quoteno);
            filecontent = filecontent.Replace("{{buyerordernodate}}", sale_quotedate);
            filecontent = filecontent.Replace("{{dispatchdocno}}", "-");
            filecontent = filecontent.Replace("{{dispatchthrough}}", deliverymode);
            filecontent = filecontent.Replace("{{dispatchdate}}", dispatchdate);

            filecontent = filecontent.Replace("{{modeofpayment}}", Sales_paymentmode);


            int initialrow = 0;

            string item_details = string.Empty;

            if (dtitem != null)
            {


                for (i = currow; i < dtitem.Rows.Count; i++)
                {

                    int checkrow = 15;

                    if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                    {
                        checkrow = 12;
                    }

                    initialrow += 1;
                    currow = i;
                    string verticalcenter = "center";
                    string str_trstyle = " style='vertical-align:" + verticalcenter + ";height:30px'";
                    if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                    {
                        str_trstyle = " style='vertical-align: top;height:60px'";
                        verticalcenter = "top";
                    }
                    if (dtitem.Rows.Count - 1 == i)
                    {
                        int totallength = 12 - initialrow;

                        if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                        {
                            totallength = 12 - initialrow;
                        }


                        if (totallength > 0)
                        {
                            verticalcenter = "top";
                            string t_length = (totallength * 50).ToString() + "px";
                            str_trstyle = " style='vertical-align:" + verticalcenter + ";height:" + t_length + @"'";

                            if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                            {
                                //str_trstyle = " style='vertical-align: " + verticalcenter + ";height:60px'";
                            }
                        }
                    }

                    string description = Convert.ToString(dtitem.Rows[i]["Name"]);
                    string HSN = Convert.ToString(dtitem.Rows[i]["HSN"]);
                    string Specification = "";
                    string SpecificationInfo = Convert.ToString(dtitem.Rows[i]["dc_serialno"]);
                    if (Specification != "")
                    {
                        description += "<br /><b>" + Specification + "</b>";
                    }
                    if (SpecificationInfo != "")
                    {
                        description += "<br /><b>" + SpecificationInfo + "</b>";
                    }

                    string sale_qty = Convert.ToString(dtitem.Rows[i]["dc_qty"]);
                    string sale_rate = Convert.ToDouble(dtitem.Rows[i]["dc_rate"]).ToString("F");
                    string sale_discount = Convert.ToDouble(dtitem.Rows[i]["dc_discount"]).ToString("F");
                    string withouttaxamt = Convert.ToDouble(dtitem.Rows[i]["withouttaxamt"]).ToString("F");
                    string taxname = Convert.ToString(dtitem.Rows[i]["taxname"]);
                    string taxrate = Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["taxrate"])).ToString("F");

                    if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                    {
                        description += "<br /><div style='text-align:right!important'>" + taxname + "</div>";
                        sale_rate += "<br /><div style='text-align:right!important'>&nbsp;</div>";
                        withouttaxamt += "<br /><div style='text-align:right!important'>" + taxrate + "</div>";
                    }

                    if (dc_method == "1")
                    {

                        if (isdiscount == true)
                        {
                            if (item_details == string.Empty)
                            {

                                item_details = @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align: " + verticalcenter + ";text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='2' style='vertical-align:" + verticalcenter + ";'>" + description + @"</td>                    
                                                         <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                            
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_discount + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                            }
                            else
                            {
                                item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='2' style='vertical-align:" + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                            
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_discount + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                            }
                        }
                        else
                        {
                            if (item_details == string.Empty)
                            {

                                item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                                                                                    
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                            }
                            else
                            {
                                item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                                                                                    
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                            }
                        }
                    }
                    else
                    {

                        if (item_details == string.Empty)
                        {

                            item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td colspan='2' style='vertical-align:" + verticalcenter + "; text-align: center'>" + HSN + @"</td>
                                                        <td colspan='2' style='vertical-align: " + verticalcenter + "; text-align: center'>" + sale_qty + @"</td>
                                                        </tr>";
                        }
                        else
                        {
                            item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td colspan='2' style='vertical-align:" + verticalcenter + "; text-align: center'>" + HSN + @"</td>
                                                        <td colspan='2' style='vertical-align: " + verticalcenter + "; text-align: center'>" + sale_qty + @"</td>
                                                        </tr>";
                        }
                    }

                    total += Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["withouttaxamt"]));
                    totalqty += Convert.ToInt32(Convert.ToString(dtitem.Rows[i]["dc_qty"]));
                    taxamt += Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["taxrate"]));
                    itemdiscount += Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["dc_discount"]));

                    if (initialrow == checkrow)
                    {
                        filecontent = filecontent.Replace("{{rowdata}}", item_details);
                        filecontent = filecontent.Replace("{{modeofpayment}}", Sales_paymentmode);





                        writer.Write(filecontent);
                        writer.Write(continuetext);

                        //writer.Write("</table>This is a Computer Generated Delivery Challan</div></div></body></html>");
                        writer.Close();
                        totalfile += 1;
                        initialrow = 0;
                        currow += 1;
                        goto ContinueFinal;
                    }

                }
            }



            filecontent = filecontent.Replace("{{rowdata}}", item_details);

            writer.Write(filecontent);
            filecontent = "";

            strpath_withdiscount = Rootpath + @"\Format\dc_invoice_discount.html";
            strpath_withoutdiscount = Rootpath + @"\Format\dc_invoice_withoutdiscount.html";
            strpath_withoutrate = Rootpath + @"\Format\dc_invoice_withoutrate.html";




            isdiscount = false;

            if (dc_method == "1")
            {

                if (discountcount != 0)
                {
                    var fileStream = new FileStream(strpath_withdiscount, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        filecontent = streamReader.ReadToEnd();
                    }

                    isdiscount = true;

                }
                else
                {
                    var fileStream = new FileStream(strpath_withoutdiscount, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        filecontent = streamReader.ReadToEnd();
                    }

                }
            }
            else
            {
                var fileStream = new FileStream(strpath_withoutrate, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    filecontent = streamReader.ReadToEnd();
                }

            }



            filecontent = filecontent.Replace("{{Terms}}", Terms);
            filecontent = filecontent.Replace("{{totalrate}}", "&nbsp;");
            filecontent = filecontent.Replace("{{totalqty}}", totalqty.ToString());
            filecontent = filecontent.Replace("{{totaldiscount}}", Convert.ToDouble(itemdiscount).ToString("F"));


            string consolidatetotal = "";
            string colscnt = "7";

            consolidatetotal = (total + taxamt).ToString();
            filecontent = filecontent.Replace("{{totalamount}}", consolidatetotal);
            double totalamt = (total + taxamt) - discount;
            string consolidatedtax = "";


            string str_consolidatetaxqry = @"select                                                 
                    isnull(STUFF((SELECT ',' + (select sub.code from tbl_taxmaster sub where  sub.id=tx.dc_tax)  + '@' + CONVERT(varchar,dc_taxvalue) + '%' 
                    FROM sandd_tbl_sales_dc_tax tx where tx.dc_id=it.dc_id and ISNULL(dc_taxvalue,0)!=0 
                                                            FOR XML PATH (''), TYPE).value('.', 'varchar(max)'), 1, 1, ''),'') as taxname,
                                                            SUM(dc_taxvalue)   as dc_taxvalue                                              
                                                              FROM sandd_tbl_sales_dc_tax it where ISNULL(dc_taxvalue,0)!=0 and dc_id=@dc_id
                                                              group by dc_id";

            str_consolidatetaxqry = @"select(select sub.code from tbl_taxmaster sub where sub.id=it.dc_tax)  + '@' +  CONVERT(varchar,dc_taxvalue)  as taxname,
                                    dc_taxvalue   as dc_taxvalue   FROM sandd_tbl_sales_dc_tax it where ISNULL(dc_taxvalue,0)!=0 and it.dc_id=@dc_id";

            string con_taxname = "";
            double con_taxvalue = 0;
            ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@dc_id", Convert.ToString(dc_id).Trim());
            dtsale = new DataTable();
            dtsale = ies.Fn_ExecutiveSql_Datatable(str_consolidatetaxqry, 0, 1);

            if (dtsale != null)
            {
                if (dtsale.Rows.Count > 0)
                {

                    for (int itaxid = 0; itaxid < dtsale.Rows.Count; itaxid++)
                    {

                        con_taxname = Convert.ToString(dtsale.Rows[itaxid]["taxname"]);
                        con_taxvalue = Convert.ToDouble(dtsale.Rows[itaxid]["dc_taxvalue"]);

                        if (con_taxname != "")
                        {
                            double totalconsolidatetaxvalue = 0;
                            if (con_taxvalue != 0)
                            {
                                totalconsolidatetaxvalue = (Convert.ToDouble(total) - Convert.ToDouble(discount)) * Convert.ToDouble(Convert.ToDouble(con_taxvalue) / Convert.ToDouble(100));
                                totalamt = totalamt + totalconsolidatetaxvalue;

                            }

                            consolidatedtax += @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>" + con_taxname + @"%&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (totalconsolidatetaxvalue).ToString("F") + "</td></tr>";


                        }

                    }
                }
            }





            filecontent = filecontent.Replace("{{consolidatetax}}", consolidatedtax);

            string consolidatediscount = "";



            if (discount != 0)
            {

                consolidatediscount = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Discount&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (discount).ToString("F") + "</td></tr>";
            }

            filecontent = filecontent.Replace("{{overalldiscount}}", consolidatediscount);












            string roundoff = roundoff = (totalamt - Math.Round(totalamt, 0)).ToString("0.00");

            if (roundoff.IndexOf("-") == 0) roundoff = roundoff.Replace('-', '+');
            else roundoff = "-" + roundoff;

            string consolidateroundoff = "";
            if (roundoff != "" && roundoff != "-0.00" && roundoff != "0.00" && roundoff != "+0.00")
            {

                consolidateroundoff = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Rounded Off&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (roundoff) + "</td></tr>";
            }


            filecontent = filecontent.Replace("{{overallroundoff}}", consolidateroundoff);



            string consolidatenettotal = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Net Total&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (Math.Round(totalamt, 0)).ToString("F") + "</td></tr>";


            filecontent = filecontent.Replace("{{nettotal}}", consolidatenettotal);







            string consolidateShipmentCharge = "";


            if (Sale_shipmentcharge != 0)
            {

                consolidateShipmentCharge = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Shipment Charge&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (Sale_shipmentcharge).ToString("F") + "</td></tr>";

            }

            filecontent = filecontent.Replace("{{shipmentcharge}}", consolidateShipmentCharge);

            double grandamt = 0;


            grandamt = totalamt + Sale_shipmentcharge;

            string consolidategrandtotal = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Grand Total&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (Math.Round(grandamt, 0)).ToString("F") + "</td></tr>";


            filecontent = filecontent.Replace("{{grandtotal}}", consolidategrandtotal);




            string amtwords;
            string totalbillamt = (Math.Round(grandamt, 0)).ToString("F");
            IndiaCurrencyConverter con = new IndiaCurrencyConverter();
            if (IsNumeric(totalbillamt))
            {
                amtwords =
                (con.ConvertToWord(totalbillamt, System.Drawing.Color.Blue, "0") + " rupees only");
            }
            else
            {
                string amt = totalbillamt.ToString().Split('.')[0];
                string paise = Convert.ToDouble(totalbillamt).ToString("0.00").Split('.')[1];

                string totalamtinwords = "";
                string totalpaiseinwords = "";
                if (Convert.ToDouble(paise) == 0)
                {
                    totalamtinwords = con.ConvertToWord(amt, System.Drawing.Color.Blue, "1") + " rupees only";
                }
                else
                {
                    totalamtinwords = con.ConvertToWord(amt, System.Drawing.Color.Blue, "1") + " rupees and ";
                    totalpaiseinwords = con.ConvertToWord(paise, System.Drawing.Color.Blue, "1") + " paise only";
                }


                amtwords = (totalamtinwords + totalpaiseinwords);
            }

            filecontent = filecontent.Replace("{{amountinwords}}", amtwords);







            writer.Write(filecontent);
            // writer.Write("</table>This is a Computer Generated Delivery Challan</div></div></body></html>");
            writer.Close();
            //MessageBox.Show(filecontent);
            // web_report.DocumentText = filecontent;
            string strpath = Rootpath + "/Invoice";
            string domainname = "0001";
            string[] Inputfile = filelist.ToArray();
            PdfGenerator.Invoicepdf(strpath, domainname,
                Inputfile);

            //pdfViewer1.Document = new PdfDocument(@"D:\Chinna\IES\Development\Xsell_Retail\Xsell_Retail\Xsell_Retail\bin\Debug\001\0001.pdf");
            string file_pathfile = strpath + "/0001.pdf";
            //axAcroPDF1.LoadFile(file_pathfile);

        }
        catch
        {

        }

        // When a cursor is back to its default shape, it means the process ends

    }

    //-------------------------------Structures and classes----------------------------

    internal bool IsNumeric(string ValueInNumeric)
    {
        bool IsFine = true;
        foreach (char ch in ValueInNumeric)
        {
            if (!(ch >= '0' && ch <= '9'))
            {
                IsFine = false;
            }
        }
        return IsFine;
    }
}




/// <summary>
/// Summary description for Invoice_Pdf
/// </summary>
public class PO_Pdf
{
    public PO_Pdf()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public PO_Pdf(string po_id, string strticketno)
    {

        string discount_print_continuetext = @"<tr>
                    <td style='vertical-align: central; text-align: left; width: 7%' colspan='7'>
                        <div style='text-align: right'><b>Continue...</b></div>
                    </td>
                </tr>";

        string withdiscount_print_continuetext = @"<tr>
                    <td style='vertical-align: central; text-align: left; width: 7%' colspan='7'>
                        <div style='text-align: right'><b>Continue.......</b></div>
                    </td>
                </tr>";




        string sale_customername = "Unknown";
        string sale_quoteno = "";
        string sale_quotedate = "";
        string deliverymode = "";
        string dispatchdate = "";
        string buyer_tinno = "";

        string str_salesdetailqry = @"select po_wardid,
                                        (select CompanyName from tbl_companysupplier c where c.company_id=po.po_supplierid) as po_customername,
                                        dbo.UDF_DateFormat(po_date,1,1) as po_date,
                                        dbo.UDF_DateFormat(po_date,1,0) as dispatchdate,
                                        isnull(po_billdiscount,0) as po_billdiscount,
                                        '0' as po_shipmentcharge,                                        
                                        po_ticketno as billNo,
                                        po_supplierid,
                                        case when ISNULL(po_deliverymode,'')='' then 'Hand Delivery' 	                                        
	                                        else ISNULL(po_deliverymode,'Hand Delivery') end as deliverymode,
                                        case when ISNULL(po_refno,'Verbal')='' then 'Verbal' else  ISNULL(po_refno,'Verbal') end  as Verbal, 
                                        case when dbo.UDF_DateFormat(ISNULL(po_refdate,'1900-01-01'),1,0)='01-01-1900'  then dbo.UDF_DateFormat(po_date,1,0) else dbo.UDF_DateFormat(po_refdate,1,0) end  as po_quotedate,
                                        isnull(po_method,'1') as po_method
                                         from sandd_tbl_purchase po where po_wardid=@po_wardid";

        string billmethod = "";
        string dc_method = "1";
        string Terms = "";
        string sale_custid = "";
        string orphan_custid = "";
        double discount = 0.0;
        double Sale_shipmentcharge = 0.0;
        string strDate = "";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@po_wardid", Convert.ToString(po_id).Trim());
        DataTable dtsale = new DataTable();
        dtsale = ies.Fn_ExecutiveSql_Datatable(str_salesdetailqry, 0, 1);

        if (dtsale != null)
        {
            if (dtsale.Rows.Count > 0)
            {
                strticketno = Convert.ToString(dtsale.Rows[0]["billNo"]);
                billmethod = Convert.ToString("Purchase Order");
                dc_method = Convert.ToString(dtsale.Rows[0]["po_method"]);
                discount = Convert.ToDouble(Convert.ToString(dtsale.Rows[0]["po_billdiscount"]));
                Sale_shipmentcharge = Convert.ToDouble(Convert.ToString(dtsale.Rows[0]["po_shipmentcharge"]));
                strDate = Convert.ToString(dtsale.Rows[0]["po_date"]);
                sale_customername = Convert.ToString(dtsale.Rows[0]["po_customername"]);

                sale_custid = Convert.ToString(dtsale.Rows[0]["po_supplierid"]);
                orphan_custid = Convert.ToString("0");
                sale_quoteno = Convert.ToString(dtsale.Rows[0]["Verbal"]);
                sale_quotedate = Convert.ToString(dtsale.Rows[0]["po_quotedate"]);
                deliverymode = Convert.ToString(dtsale.Rows[0]["deliverymode"]);
                dispatchdate = Convert.ToString(dtsale.Rows[0]["dispatchdate"]);
            }
        }



        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        string strquery = @"select top 1 * from (select distinct om.CompanyName as Company,cm.Mobile,cm.emailid,om.Area,District as[District],om.Pincode,cm.company_id,om.Address,
                                    (select st.Name from tbl_statemaster st where om.State=st.id) as State,companyno,isnull(om.TinNo,'') as TinNo,ISNULL(GSTIN,'') as GSTIN
                                    from tbl_Companysupplierscontact cm inner join tbl_companysupplier om on om.company_id=cm.company_id ) T where 1=1";
        strquery += " and company_id='" + sale_custid + "'";
        DataTable dt_companycontact = ies.Fn_ExecutiveSql_Datatable(strquery, 0, 1);


        string str_qry =
        @"			SELECT
                ROW_NUMBER() over(order by po_itemid) as slno,
                ''  as HSN,
                case 
                when po_producttype=1 then
                (
                select 
                isnull((select cn.ShortName from store_ComponentName cn where cn.ComponentID=pt.ComponentName),'NA')                                
                + '-' + isnull((select cn.ShortName from store_Packagetype cn where cn.PackageID=pt.Packagetype),'NA')    
                + '-' + isnull((select cn.PrimaryValue from store_PrimaryValue cn where cn.PrimaryID=pt.ComponentValue),'NA')     
                from store_component pt 
                 where Componentid=po_productcode)
                when po_producttype=2 then
                (select s.Name from store_Product s where Product_id=po_productcode)                
                end as Name,
                po_rate,
                po_qty,
                convert(float,((po_rate * po_qty))) as total,
                po_discount as po_discount,
                ((convert(float,it.po_rate) * convert(float,it.po_qty)) - isnull(it.po_discount,0)) as withouttaxamt,
                ((convert(float,it.po_rate) * convert(float,it.po_qty)) - isnull(it.po_discount,0))
                    + 
                isnull((select ((convert(float,it.po_rate) * convert(float,it.po_qty)) - isnull(it.po_discount,0))
                * (SUM(convert(float,po_wardtaxvalue))/100) FROM sandd_tbl_purchase_items_tax tx where tx.po_wardid=it.po_wardid and tx.po_itemid=it.po_itemid),0) as totalrate,
                isnull((select ((convert(float,it.po_rate) * convert(float,it.po_qty)) - isnull(it.po_discount,0))
                * (SUM(convert(float,tx.po_wardtaxvalue))/100) FROM sandd_tbl_purchase_items_tax tx where tx.po_wardid=it.po_wardid and tx.po_itemid=it.po_itemid),0) as taxrate,
                isnull(STUFF((SELECT ',' + (select sub.code from tbl_taxmaster sub where  sub.id=tx.po_wardtax)   + '@ ' + CONVERT(varchar,tx.po_wardtaxvalue) + '%' FROM sandd_tbl_purchase_items_tax tx where tx.po_wardid=it.po_wardid and tx.po_itemid=it.po_itemid                  
                FOR XML PATH (''), TYPE).value('.', 'varchar(max)'), 1, 1, ''),'') as taxname
                  FROM sandd_tbl_purchase_items it where po_wardid=@po_wardid";

        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@po_wardid", Convert.ToString(po_id).Trim());
        DataTable dtitem = new DataTable();
        dtitem = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);

        var discountcount = dtitem.AsEnumerable().
        Count(row => row.Field<double>("po_discount") != 0.0);

        string strbcData = strticketno;




        List<string> filelist = new List<string>();
        int totalfile = 0;
        string filefolder = HttpContext.Current.Server.MapPath("~/Invoice");
        if (System.IO.Directory.Exists(filefolder) == true)
        {
            DirectoryInfo dr = new DirectoryInfo(filefolder);
            foreach (var fi in dr.GetFiles())
            {
                File.Delete(fi.FullName);
            }
        }
        else
        {
            System.IO.Directory.CreateDirectory(filefolder);
        }



        string Rootpath = HttpContext.Current.Server.MapPath("~");

        string strpath_withdiscount = Rootpath + @"\Format\po_withdiscount.html";
        string strpath_withoutdiscount = Rootpath + @"\Format\po_withoutdiscount.html";
        string strpath_withoutrate = Rootpath + @"\Format\po_withoutrate.html";

        string filecontent;


        //Print buying goods
        double total = 0.0;

        double taxamt = 0.0;
        double itemdiscount = 0.0;

        Int32 totalqty = 0;
        int i = 0;
        int currow = 0;


        double sale_Billamt = 0;
        double receivedamt = 0;
        string Sales_paymentmode = "";

        string str_payment = @"SELECT isnull(po_paymentmode,'') as po_paymentmode  FROM sandd_tbl_purchase where po_wardid=@po_wardid";
        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@po_wardid", Convert.ToString(po_id).Trim());
        dtsale = new DataTable();
        dtsale = ies.Fn_ExecutiveSql_Datatable(str_payment, 0, 1);

        if (dtsale != null)
        {
            if (dtsale.Rows.Count > 0)
            {

                Sales_paymentmode = Convert.ToString(dtsale.Rows[0]["po_paymentmode"]);
            }
        }








    ContinueFinal:

        string filepath = filefolder + @"\" + "File_" + Convert.ToString(totalfile) + ".html";
        if (File.Exists(filepath))
        {
            File.Delete(filepath);
        }
        StreamWriter writer = new StreamWriter(filepath);
        filelist.Add(filepath);

        try
        {


            bool isdiscount = false;

            string continuetext = "";

            if (dc_method == "1")
            {

                if (discountcount != 0)
                {
                    var fileStream = new FileStream(strpath_withdiscount, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        filecontent = streamReader.ReadToEnd();
                    }

                    isdiscount = true;
                    continuetext = discount_print_continuetext;
                }
                else
                {
                    var fileStream = new FileStream(strpath_withoutdiscount, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        filecontent = streamReader.ReadToEnd();
                    }
                    continuetext = withdiscount_print_continuetext;
                }
            }
            else
            {
                var fileStream = new FileStream(strpath_withoutrate, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    filecontent = streamReader.ReadToEnd();
                }
                continuetext = discount_print_continuetext;
            }





            if (dt_companycontact != null && dt_companycontact.Rows.Count > 0)
            {
                if (dt_companycontact.Rows.Count > 0)
                {

                    buyer_tinno = Convert.ToString(dt_companycontact.Rows[0]["TinNo"]);
                    string buyer_GSTIN = Convert.ToString(dt_companycontact.Rows[0]["GSTIN"]);
                    filecontent = filecontent.Replace("{{companyname}}", Convert.ToString(dt_companycontact.Rows[0]["Company"]));


                    filecontent = filecontent.Replace("{{area}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));

                    filecontent = filecontent.Replace("{{state}}", Convert.ToString(dt_companycontact.Rows[0]["District"]) + "," + Convert.ToString(dt_companycontact.Rows[0]["state"]));
                    if (buyer_tinno != "" && buyer_GSTIN != "")
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".<br>" + "Buyer's Sales Tax No." + buyer_tinno + "<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                    }
                    else if (buyer_tinno == "" && buyer_GSTIN != "")
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                    }
                    else
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".");
                    }

                    filecontent = filecontent.Replace("{{address}}", Convert.ToString(dt_companycontact.Rows[0]["address"]));

                    filecontent = filecontent.Replace("{{customerrefno}}", Convert.ToString(dt_companycontact.Rows[0]["companyno"]));

                    filecontent = filecontent.Replace("{{otherref}}", "-");
                    filecontent = filecontent.Replace("{{deliverynote}}", "-");
                    filecontent = filecontent.Replace("{{dispatchto}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));



                }

            }
            else
            {
                filecontent = filecontent.Replace("{{companyname}}", sale_customername);

                filecontent = filecontent.Replace("<div>{{area}},</div>", "");
                filecontent = filecontent.Replace("<div>{{state}} - {{postal}}.</div>", "");
                filecontent = filecontent.Replace("<div>{{address}},</div>", "");


                filecontent = filecontent.Replace("{{postal}}", "&nbsp;");
                filecontent = filecontent.Replace("<div>{{area}},</div>", "");
                filecontent = filecontent.Replace("{{area}}", "&nbsp;");
                filecontent = filecontent.Replace("{{state}}", "&nbsp;");
                filecontent = filecontent.Replace("<div>{{address}},</div>", "");
                filecontent = filecontent.Replace("{{address}}", "&nbsp;");
                filecontent = filecontent.Replace("{{customerrefno}}", "-");
                filecontent = filecontent.Replace("{{otherref}}", "-");
                filecontent = filecontent.Replace("{{deliverynote}}", "-");
                filecontent = filecontent.Replace("{{dispatchto}}", "-");
            }


            filecontent = filecontent.Replace("{{invoiceno}}", strticketno);
            filecontent = filecontent.Replace("{{invoicedate}}", strDate);
            filecontent = filecontent.Replace("{{deliverynote}}", "-");
            filecontent = filecontent.Replace("{{buyerorderno}}", sale_quoteno);
            filecontent = filecontent.Replace("{{buyerordernodate}}", sale_quotedate);
            filecontent = filecontent.Replace("{{dispatchdocno}}", "-");
            filecontent = filecontent.Replace("{{dispatchthrough}}", deliverymode);
            filecontent = filecontent.Replace("{{dispatchdate}}", dispatchdate);

            filecontent = filecontent.Replace("{{modeofpayment}}", Sales_paymentmode);


            int initialrow = 0;

            string item_details = string.Empty;

            if (dtitem != null)
            {


                for (i = currow; i < dtitem.Rows.Count; i++)
                {

                    int checkrow = 15;

                    if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                    {
                        checkrow = 12;
                    }

                    initialrow += 1;
                    currow = i;
                    string verticalcenter = "center";
                    string str_trstyle = " style='vertical-align:" + verticalcenter + ";height:30px'";
                    if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                    {
                        str_trstyle = " style='vertical-align: top;height:60px'";
                        verticalcenter = "top";
                    }
                    if (dtitem.Rows.Count - 1 == i)
                    {
                        int totallength = 15 - initialrow;

                        if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                        {
                            totallength = 12 - initialrow;
                        }


                        if (totallength > 0)
                        {
                            verticalcenter = "top";
                            string t_length = (totallength * 25).ToString() + "px";
                            str_trstyle = " style='vertical-align:" + verticalcenter + ";height:" + t_length + @"'";

                            if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                            {
                                //str_trstyle = " style='vertical-align: " + verticalcenter + ";height:60px'";
                            }
                        }
                    }

                    string description = Convert.ToString(dtitem.Rows[i]["Name"]);
                    string HSN = Convert.ToString(dtitem.Rows[i]["HSN"]);
                    string Specification = "";
                    string SpecificationInfo = "";
                    if (Specification != "")
                    {
                        description += "<br /><b>" + Specification + "</b>";
                    }
                    if (SpecificationInfo != "")
                    {
                        description += "<br /><b>" + SpecificationInfo + "</b>";
                    }

                    string sale_qty = Convert.ToString(dtitem.Rows[i]["po_qty"]);
                    string sale_rate = Convert.ToDouble(dtitem.Rows[i]["po_rate"]).ToString("F");
                    string sale_discount = Convert.ToDouble(dtitem.Rows[i]["po_discount"]).ToString("F");
                    string withouttaxamt = Convert.ToDouble(dtitem.Rows[i]["withouttaxamt"]).ToString("F");
                    string taxname = Convert.ToString(dtitem.Rows[i]["taxname"]);
                    string taxrate = Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["taxrate"])).ToString("F");

                    if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                    {
                        description += "<br /><div style='text-align:right!important'>" + taxname + "</div>";
                        sale_rate += "<br /><div style='text-align:right!important'>&nbsp;</div>";
                        withouttaxamt += "<br /><div style='text-align:right!important'>" + taxrate + "</div>";
                    }

                    if (dc_method == "1")
                    {

                        if (isdiscount == true)
                        {
                            if (item_details == string.Empty)
                            {

                                item_details = @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align: " + verticalcenter + ";text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align:" + verticalcenter + ";'>" + description + @"</td>                                                                             
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                            
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_discount + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                            }
                            else
                            {
                                item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align:" + verticalcenter + ";'>" + description + @"</td>                                                                            
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                            
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_discount + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                            }
                        }
                        else
                        {
                            if (item_details == string.Empty)
                            {

                                item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                                                                            
                                                        <td colspan='2' style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                                                                                    
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                            }
                            else
                            {
                                item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                                                                            
                                                        <td colspan='2' style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                                                                                    
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                            }
                        }
                    }
                    else
                    {

                        if (item_details == string.Empty)
                        {

                            item_details += @"<tr" + str_trstyle + @">
                                                        <td colspan='2' style='vertical-align:" + verticalcenter + "; text-align: center;'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='4' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                                                                           
                                                        <td colspan='2' style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        </tr>";
                        }
                        else
                        {
                            item_details += @"<tr" + str_trstyle + @">
                                                        <td colspan='2' style='vertical-align: " + verticalcenter + "; text-align: center;'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='4' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                                                                            
                                                        <td colspan='2' style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        </tr>";
                        }
                    }

                    total += Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["withouttaxamt"]));
                    totalqty += Convert.ToInt32(Convert.ToString(dtitem.Rows[i]["po_qty"]));
                    taxamt += Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["taxrate"]));
                    itemdiscount += Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["po_discount"]));

                    if (initialrow == checkrow)
                    {
                        filecontent = filecontent.Replace("{{rowdata}}", item_details);
                        filecontent = filecontent.Replace("{{modeofpayment}}", Sales_paymentmode);





                        writer.Write(filecontent);
                        writer.Write(continuetext);

                        writer.Write("</table>This is a Computer Generated Purchase Order</div></div></body></html>");
                        writer.Close();
                        totalfile += 1;
                        initialrow = 0;
                        currow += 1;
                        goto ContinueFinal;
                    }

                }
            }



            filecontent = filecontent.Replace("{{rowdata}}", item_details);

            writer.Write(filecontent);
            filecontent = "";

            strpath_withdiscount = Rootpath + @"\Format\po_dec_withdiscount.html";
            strpath_withoutdiscount = Rootpath + @"\Format\po_dec_withoutdiscount.html";
            strpath_withoutrate = Rootpath + @"\Format\po_dec_withoutrate.html";




            isdiscount = false;

            if (dc_method == "1")
            {

                if (discountcount != 0)
                {
                    var fileStream = new FileStream(strpath_withdiscount, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        filecontent = streamReader.ReadToEnd();
                    }

                    isdiscount = true;

                }
                else
                {
                    var fileStream = new FileStream(strpath_withoutdiscount, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        filecontent = streamReader.ReadToEnd();
                    }

                }
            }
            else
            {
                var fileStream = new FileStream(strpath_withoutrate, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    filecontent = streamReader.ReadToEnd();
                }

            }



            filecontent = filecontent.Replace("{{Terms}}", Terms);
            filecontent = filecontent.Replace("{{totalrate}}", "&nbsp;");
            filecontent = filecontent.Replace("{{totalqty}}", totalqty.ToString());
            filecontent = filecontent.Replace("{{totaldiscount}}", Convert.ToDouble(itemdiscount).ToString("F"));


            string consolidatetotal = "";
            string colscnt = "7";

            consolidatetotal = (total + taxamt).ToString();
            filecontent = filecontent.Replace("{{totalamount}}", consolidatetotal);
            double totalamt = (total + taxamt) - discount;
            string consolidatedtax = "";


            string str_consolidatetaxqry = @"select isnull(STUFF((SELECT ',' + (select sub.code from tbl_taxmaster sub where  sub.id=tx.po_wardtax)  + '@' + CONVERT(varchar,po_wardtaxvalue) + '%' 
                    FROM sandd_tbl_purchase_tax tx where tx.po_wardid=it.po_wardid and ISNULL(po_wardtaxvalue,0)!=0 
                                                            FOR XML PATH (''), TYPE).value('.', 'varchar(max)'), 1, 1, ''),'') as taxname,
                                                            SUM(po_wardtaxvalue)   as po_taxvalue                                              
                                                              FROM sandd_tbl_purchase_tax it where ISNULL(po_wardtaxvalue,0)!=0 and po_wardid=@po_wardid
                                                              group by po_wardid";

            str_consolidatetaxqry = @"select(select sub.code from tbl_taxmaster sub where sub.id=it.po_wardtax)  + '@' +  CONVERT(varchar,po_wardtaxvalue)  as taxname,
                                    po_wardtaxvalue   as po_taxvalue   FROM sandd_tbl_purchase_tax it where ISNULL(po_wardtaxvalue,0)!=0 and it.po_wardid=@po_wardid";

            string con_taxname = "";
            double con_taxvalue = 0;
            ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@po_wardid", Convert.ToString(po_id).Trim());
            dtsale = new DataTable();
            dtsale = ies.Fn_ExecutiveSql_Datatable(str_consolidatetaxqry, 0, 1);

            if (dtsale != null)
            {
                if (dtsale.Rows.Count > 0)
                {

                    for (int itaxid = 0; itaxid < dtsale.Rows.Count; itaxid++)
                    {

                        con_taxname = Convert.ToString(dtsale.Rows[itaxid]["taxname"]);
                        con_taxvalue = Convert.ToDouble(dtsale.Rows[itaxid]["po_taxvalue"]);

                        if (con_taxname != "")
                        {
                            double totalconsolidatetaxvalue = 0;
                            if (con_taxvalue != 0)
                            {
                                totalconsolidatetaxvalue = (Convert.ToDouble(total) - Convert.ToDouble(discount)) * Convert.ToDouble(Convert.ToDouble(con_taxvalue) / Convert.ToDouble(100));
                                totalamt = totalamt + totalconsolidatetaxvalue;

                            }

                            consolidatedtax += @"<tr>
                                        <td style='vertical-align: central; text-align: right;' colspan='" + colscnt + @"'>" + con_taxname + @"%&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (totalconsolidatetaxvalue).ToString("F") + "</td></tr>";


                        }

                    }
                }
            }





            filecontent = filecontent.Replace("{{consolidatetax}}", consolidatedtax);

            string consolidatediscount = "";



            if (discount != 0)
            {

                consolidatediscount = @"<tr>
                                        <td style='vertical-align: central; text-align: right;' colspan='" + colscnt + @"'>Discount&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (discount).ToString("F") + "</td></tr>";
            }

            filecontent = filecontent.Replace("{{overalldiscount}}", consolidatediscount);












            string roundoff = roundoff = (totalamt - Math.Round(totalamt, 0)).ToString("0.00");

            if (roundoff.IndexOf("-") == 0) roundoff = roundoff.Replace('-', '+');
            else roundoff = "-" + roundoff;

            string consolidateroundoff = "";
            if (roundoff != "" && roundoff != "-0.00" && roundoff != "0.00" && roundoff != "+0.00")
            {

                consolidateroundoff = @"<tr>
                                        <td style='vertical-align: central; text-align: right;' colspan='" + colscnt + @"'>Rounded Off&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (roundoff) + "</td></tr>";
            }


            filecontent = filecontent.Replace("{{overallroundoff}}", consolidateroundoff);



            string consolidatenettotal = @"<tr>
                                        <td style='vertical-align: central; text-align: right;' colspan='" + colscnt + @"'>Net Total&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (Math.Round(totalamt, 0)).ToString("F") + "</td></tr>";


            filecontent = filecontent.Replace("{{nettotal}}", consolidatenettotal);







            string consolidateShipmentCharge = "";


            if (Sale_shipmentcharge != 0)
            {

                consolidateShipmentCharge = @"<tr>
                                        <td style='vertical-align: central; text-align: right;' colspan='" + colscnt + @"'>Shipment Charge&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (Sale_shipmentcharge).ToString("F") + "</td></tr>";

            }

            filecontent = filecontent.Replace("{{shipmentcharge}}", consolidateShipmentCharge);

            double grandamt = 0;


            grandamt = totalamt + Sale_shipmentcharge;

            string consolidategrandtotal = @"<tr>
                                        <td style='vertical-align: central; text-align: right;' colspan='" + colscnt + @"'>Grand Total&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (Math.Round(grandamt, 0)).ToString("F") + "</td></tr>";


            filecontent = filecontent.Replace("{{grandtotal}}", consolidategrandtotal);




            string amtwords;
            string totalbillamt = (Math.Round(grandamt, 0)).ToString("F");
            IndiaCurrencyConverter con = new IndiaCurrencyConverter();
            if (IsNumeric(totalbillamt))
            {
                amtwords =
                (con.ConvertToWord(totalbillamt, System.Drawing.Color.Blue, "0") + " rupees only");
            }
            else
            {
                string amt = totalbillamt.ToString().Split('.')[0];
                string paise = Convert.ToDouble(totalbillamt).ToString("0.00").Split('.')[1];

                string totalamtinwords = "";
                string totalpaiseinwords = "";
                if (Convert.ToDouble(paise) == 0)
                {
                    totalamtinwords = con.ConvertToWord(amt, System.Drawing.Color.Blue, "1") + " rupees only";
                }
                else
                {
                    totalamtinwords = con.ConvertToWord(amt, System.Drawing.Color.Blue, "1") + " rupees and ";
                    totalpaiseinwords = con.ConvertToWord(paise, System.Drawing.Color.Blue, "1") + " paise only";
                }


                amtwords = (totalamtinwords + totalpaiseinwords);
            }

            filecontent = filecontent.Replace("{{amountinwords}}", amtwords);







            writer.Write(filecontent);
            writer.Write("</table>This is a Computer Generated Purchase Order</div></div></body></html>");
            writer.Close();
            //MessageBox.Show(filecontent);
            // web_report.DocumentText = filecontent;
            string strpath = Rootpath + "/Invoice";
            string domainname = "0001";
            string[] Inputfile = filelist.ToArray();
            PdfGenerator.Invoicepdf(strpath, domainname,
                Inputfile);

            //pdfViewer1.Document = new PdfDocument(@"D:\Chinna\IES\Development\Xsell_Retail\Xsell_Retail\Xsell_Retail\bin\Debug\001\0001.pdf");
            string file_pathfile = strpath + "/0001.pdf";
            //axAcroPDF1.LoadFile(file_pathfile);

        }
        catch
        {

        }

        // When a cursor is back to its default shape, it means the process ends

    }

    //-------------------------------Structures and classes----------------------------

    internal bool IsNumeric(string ValueInNumeric)
    {
        bool IsFine = true;
        foreach (char ch in ValueInNumeric)
        {
            if (!(ch >= '0' && ch <= '9'))
            {
                IsFine = false;
            }
        }
        return IsFine;
    }
}



/// <summary>
/// Summary description for Invoice_Pdf
/// </summary>
public class Performa_Pdf
{
    public Performa_Pdf()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public Performa_Pdf(string performa_wardid, string strticketno, string printmode)
    {

        string discount_print_continuetext = @"<tr>
                    <td style='vertical-align: central; text-align: left; width: 7%' colspan='7'>
                        <div style='text-align: right'><b>Continue...</b></div>
                    </td>
                </tr>";

        string withdiscount_print_continuetext = @"<tr>
                    <td style='vertical-align: central; text-align: left; width: 7%' colspan='7'>
                        <div style='text-align: right'><b>Continue.......</b></div>
                    </td>
                </tr>";




        string sale_customername = "Unknown";
        string sale_quoteno = "";
        string sale_quotedate = "";
        string deliverymode = "";
        string dispatchdate = "";
        string buyer_tinno = "";

        double sale_Billamt = 0;
        double receivedamt = 0;
        string Sales_paymentmode = "";

        string str_salesdetailqry = @"select 
                                        performa_wardid,performa_customername,performa_paymentmode,
                                        dbo.UDF_DateFormat(performa_date,1,1) as sale_date,
                                        dbo.UDF_DateFormat(performa_date,1,0) as dispatchdate,
                                        isnull(performa_billdiscount,0) as sale_billdiscount,
                                        isnull(performa_shipmentcharge,0) as Sale_shipmentcharge,
                                        case when ISNULL(performa_billmethod,'0')='1' then 'PERFORMA INVOICE' else 'SALES QUOTATION' end as sale_billmethod,
                                        performa_ticketno as billNo,
                                        financialyear,performa_customerid,
                                        case when ISNULL(performa_deliverymode,'')='' then 'Hand Delivery' 	                                        
	                                        else ISNULL(performa_deliverymode,'Hand Delivery') end as deliverymode,
                                        case when ISNULL(performa_refno,'Verbal')='' then 'Verbal' else  ISNULL(performa_refno,'Verbal') end  as Verbal, 
                                        case when dbo.UDF_DateFormat(ISNULL(performa_refdate,'1900-01-01'),1,0)='01-01-1900'  then dbo.UDF_DateFormat(performa_date,1,0) else dbo.UDF_DateFormat(performa_refdate,1,0) end  as performa_refdate
                                        ,isnull(orphan_id,'0') as orphan_id
                                         from sandd_tbl_performa where performa_wardid=@performa_wardid";

        string billmethod = "";
        string sale_custid = "";
        string orphan_custid = "";
        double discount = 0.0;
        double Sale_shipmentcharge = 0.0;
        string strDate = "";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@performa_wardid", Convert.ToString(performa_wardid).Trim());
        DataTable dtsale = new DataTable();
        dtsale = ies.Fn_ExecutiveSql_Datatable(str_salesdetailqry, 0, 1);

        if (dtsale != null)
        {
            if (dtsale.Rows.Count > 0)
            {
                strticketno = Convert.ToString(dtsale.Rows[0]["billNo"]);
                billmethod = Convert.ToString(dtsale.Rows[0]["sale_billmethod"]);
                Sales_paymentmode = Convert.ToString(dtsale.Rows[0]["performa_paymentmode"]);
                discount = Convert.ToDouble(Convert.ToString(dtsale.Rows[0]["sale_billdiscount"]));
                Sale_shipmentcharge = Convert.ToDouble(Convert.ToString(dtsale.Rows[0]["Sale_shipmentcharge"]));
                strDate = Convert.ToString(dtsale.Rows[0]["sale_date"]);
                sale_customername = Convert.ToString(dtsale.Rows[0]["performa_customername"]);

                sale_custid = Convert.ToString(dtsale.Rows[0]["performa_customerid"]);
                orphan_custid = Convert.ToString(dtsale.Rows[0]["orphan_id"]);
                sale_quoteno = Convert.ToString(dtsale.Rows[0]["Verbal"]);
                sale_quotedate = Convert.ToString(dtsale.Rows[0]["performa_refdate"]);
                deliverymode = Convert.ToString(dtsale.Rows[0]["deliverymode"]);
                dispatchdate = Convert.ToString(dtsale.Rows[0]["dispatchdate"]);
            }
        }



        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        string strquery = @"select top 1 * from (select distinct om.CompanyName as Company,cm.Mobile,cm.emailid,om.Area,District as[District],om.Pincode,cm.company_id,om.Address,
                                    (select st.Name from tbl_statemaster st where om.State=st.id) as State,companyno,isnull(om.TinNo,'') as TinNo,ISNULL(GSTIN,'') as GSTIN
                                    from tbl_Companycustomercontact cm inner join tbl_companycustomer om on om.company_id=cm.company_id ) T where 1=1";
        strquery += " and company_id='" + sale_custid + "'";
        DataTable dt_companycontact = ies.Fn_ExecutiveSql_Datatable(strquery, 0, 1);


        string str_qry =
        @"SELECT ROW_NUMBER() over(order by performa_itemid) as slno,
                case when ISNULL(hsn_code,'')='' then '' else  ISNULL(hsn_code,'')  end  as HSN,
                case 
                when performa_producttype=1 then
                (
                select 
                isnull((select cn.ShortName from store_ComponentName cn where cn.ComponentID=pt.ComponentName),'NA')                                
                + '-' + isnull((select cn.ShortName from store_Packagetype cn where cn.PackageID=pt.Packagetype),'NA')    
                + '-' + isnull((select cn.PrimaryValue from store_PrimaryValue cn where cn.PrimaryID=pt.ComponentValue),'NA')     
                from store_component pt 
                 where Componentid=performa_productcode)
                when performa_producttype=2 then
                (select s.Name from store_Product s where Product_id=performa_productcode)                
                end as Name,  
                '' as Specification,
                ''  as SpecificationInfo,                
                performa_rate,
                performa_qty,
                convert(float,((performa_rate * performa_qty))) as total,
                performa_discount as performa_discount,
                ((convert(float,it.performa_rate) * convert(float,it.performa_qty)) - isnull(it.performa_discount,0)) as withouttaxamt,
                ((convert(float,it.performa_rate) * convert(float,it.performa_qty)) - isnull(it.performa_discount,0))
                    + 
                isnull((select ((convert(float,it.performa_rate) * convert(float,it.performa_qty)) - isnull(it.performa_discount,0))
                * (SUM(convert(float,performa_wardtaxvalue))/100) FROM sandd_tbl_performa_items_tax tx where tx.performa_wardid=it.performa_wardid and tx.performa_itemid=it.performa_itemid),0) as totalrate,
                isnull((select ((convert(float,it.performa_rate) * convert(float,it.performa_qty)) - isnull(it.performa_discount,0))
                * (SUM(convert(float,performa_wardtaxvalue))/100) FROM sandd_tbl_performa_items_tax tx where tx.performa_wardid=it.performa_wardid and tx.performa_itemid=it.performa_itemid),0) as taxrate,
                isnull(STUFF((SELECT ',' + (select sub.code from tbl_taxmaster sub where  sub.id=tx.performa_wardtax)   + '@ ' + CONVERT(varchar,performa_wardtaxvalue) + '%' FROM sandd_tbl_performa_items_tax tx where tx.performa_wardid=it.performa_wardid and tx.performa_itemid=it.performa_itemid                  
                FOR XML PATH (''), TYPE).value('.', 'varchar(max)'), 1, 1, ''),'') as taxname
                  FROM sandd_tbl_performa_items it where performa_wardid=@performa_wardid";

        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@performa_wardid", Convert.ToString(performa_wardid).Trim());
        DataTable dtitem = new DataTable();
        dtitem = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);

        var discountcount = dtitem.AsEnumerable().
        Count(row => row.Field<double>("performa_discount") != 0.0);

        string strbcData = strticketno;




        List<string> filelist = new List<string>();
        int totalfile = 0;
        string filefolder = HttpContext.Current.Server.MapPath("~/Invoice");
        if (System.IO.Directory.Exists(filefolder) == true)
        {
            DirectoryInfo dr = new DirectoryInfo(filefolder);
            foreach (var fi in dr.GetFiles())
            {
                File.Delete(fi.FullName);
            }
        }
        else
        {
            System.IO.Directory.CreateDirectory(filefolder);
        }



        string Rootpath = HttpContext.Current.Server.MapPath("~");

        string strpath_withdiscount = Rootpath + @"\Format\performa_finalwithdiscount.html";
        string strpath_withoutdiscount = Rootpath + @"\Format\performa_finalwithoutdiscount.html";

        string filecontent;


        //Print buying goods
        double total = 0.0;

        double taxamt = 0.0;
        double itemdiscount = 0.0;

        Int32 totalqty = 0;
        int i = 0;
        int currow = 0;










    ContinueFinal:

        string filepath = filefolder + @"\" + "File_" + Convert.ToString(totalfile) + ".html";
        if (File.Exists(filepath))
        {
            File.Delete(filepath);
        }
        StreamWriter writer = new StreamWriter(filepath);
        filelist.Add(filepath);

        try
        {


            bool isdiscount = false;

            string continuetext = "";

            if (discountcount != 0)
            {
                var fileStream = new FileStream(strpath_withdiscount, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    filecontent = streamReader.ReadToEnd();
                }

                isdiscount = true;
                continuetext = discount_print_continuetext;
            }
            else
            {
                var fileStream = new FileStream(strpath_withoutdiscount, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    filecontent = streamReader.ReadToEnd();
                }
                continuetext = withdiscount_print_continuetext;
            }





            if (dt_companycontact != null)
            {
                if (dt_companycontact.Rows.Count > 0)
                {

                    buyer_tinno = Convert.ToString(dt_companycontact.Rows[0]["TinNo"]);
                    string buyer_GSTIN = Convert.ToString(dt_companycontact.Rows[0]["GSTIN"]);
                    filecontent = filecontent.Replace("{{companyname}}", Convert.ToString(dt_companycontact.Rows[0]["Company"]));


                    filecontent = filecontent.Replace("{{area}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));

                    filecontent = filecontent.Replace("{{state}}", Convert.ToString(dt_companycontact.Rows[0]["District"]) + "," + Convert.ToString(dt_companycontact.Rows[0]["state"]));
                    if (buyer_tinno != "" && buyer_GSTIN != "")
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".<br>" + "Buyer's Sales Tax No." + buyer_tinno + "<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                    }
                    else if (buyer_tinno == "" && buyer_GSTIN != "")
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                    }
                    else
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".");
                    }

                    filecontent = filecontent.Replace("{{address}}", Convert.ToString(dt_companycontact.Rows[0]["address"]));

                    filecontent = filecontent.Replace("{{customerrefno}}", Convert.ToString(dt_companycontact.Rows[0]["companyno"]));

                    filecontent = filecontent.Replace("{{otherref}}", "-");
                    filecontent = filecontent.Replace("{{deliverynote}}", "-");
                    filecontent = filecontent.Replace("{{dispatchto}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));



                }
                else
                {

                    ies = new IES_Generic_Function();
                    ies.ies_parameters.Clear();
                    strquery = @"select isnull(om.CompanyName,'') as Company,
                                    ISNULL(om.Mobileno,'') as Mobile,isnull(om.emailid,'') as emailid,
                                    isnull(om.Area,'') as Area,
                                    isnull(District,'') as[District],
                                    isnull(om.Pincode,'') as Pincode,'' as Address,
                                    '' as State,'' as companyno,
                                    isnull(om.TinNo,'') as TinNo,
                                    ISNULL(GSTIN,'') as GSTIN,
                                    '0' as company_id
                                    from tbl_orphancustomer om where 1=1 ";
                    strquery += " and company_id='" + orphan_custid + "'";
                    dt_companycontact = ies.Fn_ExecutiveSql_Datatable(strquery, 0, 1);


                    if (dt_companycontact != null)
                    {
                        if (dt_companycontact.Rows.Count > 0)
                        {

                            buyer_tinno = Convert.ToString(dt_companycontact.Rows[0]["TinNo"]);
                            string buyer_GSTIN = Convert.ToString(dt_companycontact.Rows[0]["GSTIN"]);
                            filecontent = filecontent.Replace("{{companyname}}", Convert.ToString(dt_companycontact.Rows[0]["Company"]));
                            if (Convert.ToString(dt_companycontact.Rows[0]["area"]) == "")
                            {
                                filecontent = filecontent.Replace("{{area}},", "");
                            }
                            else
                            {
                                filecontent = filecontent.Replace("{{area}},", Convert.ToString(dt_companycontact.Rows[0]["area"]) + ",");
                            }
                            if (Convert.ToString(dt_companycontact.Rows[0]["District"]) == "")
                            {
                                filecontent = filecontent.Replace("{{state}} - ", "");
                            }
                            else
                            {
                                filecontent = filecontent.Replace("{{state}} - ", Convert.ToString(dt_companycontact.Rows[0]["District"]) + ",");
                            }

                            if (buyer_tinno != "" && buyer_GSTIN != "")
                            {
                                filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + "<br>" + "Buyer's Sales Tax No." + buyer_tinno + "<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                            }
                            else if (buyer_tinno == "" && buyer_GSTIN != "")
                            {
                                filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + "<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                            }
                            else
                            {

                                if (Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) == "")
                                {
                                    filecontent = filecontent.Replace("{{postal}}", "");
                                }
                                else
                                {
                                    filecontent = filecontent.Replace("{{postal}}", "<br>" + Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".");
                                }

                            }
                            filecontent = filecontent.Replace("{{dispatchto}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));
                        }
                    }

                    filecontent = filecontent.Replace("{{companyname}}", sale_customername);
                    filecontent = filecontent.Replace("{{postal}}", "&nbsp;");
                    filecontent = filecontent.Replace("{{area}}", "&nbsp;");
                    filecontent = filecontent.Replace("{{area}},", "&nbsp;");
                    filecontent = filecontent.Replace("{{state}}", "&nbsp;");
                    filecontent = filecontent.Replace("<div>{{address}},</div>", "");
                    filecontent = filecontent.Replace("{{address}}", "&nbsp;");
                    filecontent = filecontent.Replace("{{address}},", "&nbsp;");
                    filecontent = filecontent.Replace("{{customerrefno}}", "-");
                    filecontent = filecontent.Replace("{{otherref}}", "-");
                    filecontent = filecontent.Replace("{{deliverynote}}", "-");
                    filecontent = filecontent.Replace("{{dispatchto}}", "-");
                }
            }
            else
            {
                filecontent = filecontent.Replace("{{companyname}}", sale_customername);

                filecontent = filecontent.Replace("<div>{{area}},</div>", "");
                filecontent = filecontent.Replace("<div>{{state}} - {{postal}}.</div>", "");
                filecontent = filecontent.Replace("<div>{{address}},</div>", "");


                filecontent = filecontent.Replace("{{postal}}", "&nbsp;");
                filecontent = filecontent.Replace("<div>{{area}},</div>", "");
                filecontent = filecontent.Replace("{{area}}", "&nbsp;");
                filecontent = filecontent.Replace("{{state}}", "&nbsp;");
                filecontent = filecontent.Replace("<div>{{address}},</div>", "");
                filecontent = filecontent.Replace("{{address}}", "&nbsp;");
                filecontent = filecontent.Replace("{{customerrefno}}", "-");
                filecontent = filecontent.Replace("{{otherref}}", "-");
                filecontent = filecontent.Replace("{{deliverynote}}", "-");
                filecontent = filecontent.Replace("{{dispatchto}}", "-");
            }


            filecontent = filecontent.Replace("{{ORIGINAL}}", printmode);
            filecontent = filecontent.Replace("{{invoiceno}}", strticketno);
            filecontent = filecontent.Replace("{{invoiceno}}", strticketno);
            filecontent = filecontent.Replace("{{invoicedate}}", strDate);
            filecontent = filecontent.Replace("{{deliverynote}}", "-");
            filecontent = filecontent.Replace("{{buyerorderno}}", sale_quoteno);
            filecontent = filecontent.Replace("{{buyerordernodate}}", sale_quotedate);
            filecontent = filecontent.Replace("{{dispatchdocno}}", "-");
            filecontent = filecontent.Replace("{{dispatchthrough}}", deliverymode);
            filecontent = filecontent.Replace("{{dispatchdate}}", dispatchdate);

            filecontent = filecontent.Replace("{{modeofpayment}}", Sales_paymentmode);


            int initialrow = 0;

            string item_details = string.Empty;

            if (dtitem != null)
            {


                for (i = currow; i < dtitem.Rows.Count; i++)
                {

                    int checkrow = 15;

                    if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                    {
                        checkrow = 12;
                    }

                    initialrow += 1;
                    currow = i;
                    string verticalcenter = "center";
                    string str_trstyle = " style='vertical-align:" + verticalcenter + ";height:30px'";
                    if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                    {
                        str_trstyle = " style='vertical-align: top;height:60px'";
                        verticalcenter = "top";
                    }
                    if (dtitem.Rows.Count - 1 == i)
                    {
                        int totallength = 15 - initialrow;

                        if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                        {
                            totallength = 12 - initialrow;
                        }


                        if (totallength > 0)
                        {
                            verticalcenter = "top";
                            string t_length = (totallength * 25).ToString() + "px";
                            str_trstyle = " style='vertical-align:" + verticalcenter + ";height:" + t_length + @"'";

                            if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                            {
                                //str_trstyle = " style='vertical-align: " + verticalcenter + ";height:60px'";
                            }
                        }
                    }

                    string description = Convert.ToString(dtitem.Rows[i]["Name"]);
                    string HSN = Convert.ToString(dtitem.Rows[i]["HSN"]);
                    string Specification = Convert.ToString(dtitem.Rows[i]["Specification"]).Trim();
                    string SpecificationInfo = Convert.ToString(dtitem.Rows[i]["SpecificationInfo"]).Trim();
                    if (Specification != "")
                    {
                        description += "<br /><b>" + Specification + "</b>";
                    }
                    if (SpecificationInfo != "")
                    {
                        description += "<br /><b>" + SpecificationInfo + "</b>";
                    }

                    string sale_qty = Convert.ToString(dtitem.Rows[i]["performa_qty"]);
                    string sale_rate = Convert.ToDouble(dtitem.Rows[i]["performa_rate"]).ToString("F");
                    string sale_discount = Convert.ToDouble(dtitem.Rows[i]["performa_discount"]).ToString("F");
                    string withouttaxamt = Convert.ToDouble(dtitem.Rows[i]["withouttaxamt"]).ToString("F");
                    string taxname = Convert.ToString(dtitem.Rows[i]["taxname"]);
                    string taxrate = Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["taxrate"])).ToString("F");

                    if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                    {
                        description += "<br /><div style='text-align:right!important'>" + taxname + "</div>";
                        sale_rate += "<br /><div style='text-align:right!important'>&nbsp;</div>";
                        withouttaxamt += "<br /><div style='text-align:right!important'>" + taxrate + "</div>";
                    }

                    if (isdiscount == true)
                    {
                        if (item_details == string.Empty)
                        {

                            item_details = @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align: " + verticalcenter + ";text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='2' style='vertical-align:" + verticalcenter + ";'>" + description + @"</td>                    
                                                         <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                            
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_discount + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                        }
                        else
                        {
                            item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='2' style='vertical-align:" + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                            
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_discount + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                        }
                    }
                    else
                    {
                        if (item_details == string.Empty)
                        {

                            item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                                                                                    
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                        }
                        else
                        {
                            item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                                                                                    
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                        }
                    }

                    total += Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["withouttaxamt"]));
                    totalqty += Convert.ToInt32(Convert.ToString(dtitem.Rows[i]["performa_qty"]));
                    taxamt += Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["taxrate"]));
                    itemdiscount += Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["performa_discount"]));

                    if (initialrow == checkrow)
                    {
                        filecontent = filecontent.Replace("{{rowdata}}", item_details);
                        filecontent = filecontent.Replace("{{modeofpayment}}", Sales_paymentmode);





                        writer.Write(filecontent);
                        writer.Write(continuetext);


                        writer.Close();
                        totalfile += 1;
                        initialrow = 0;
                        currow += 1;
                        goto ContinueFinal;
                    }

                }
            }



            filecontent = filecontent.Replace("{{rowdata}}", item_details);

            writer.Write(filecontent);
            filecontent = "";

            strpath_withdiscount = Rootpath + @"\Format\performa_discount.html";
            strpath_withoutdiscount = Rootpath + @"\Format\performa_withoutdiscount.html";



            isdiscount = false;



            if (discountcount != 0)
            {
                var fileStream = new FileStream(strpath_withdiscount, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    filecontent = streamReader.ReadToEnd();
                }

                isdiscount = true;

            }
            else
            {
                var fileStream = new FileStream(strpath_withoutdiscount, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    filecontent = streamReader.ReadToEnd();
                }

            }




            filecontent = filecontent.Replace("{{totalrate}}", "&nbsp;");
            filecontent = filecontent.Replace("{{totalqty}}", totalqty.ToString());
            filecontent = filecontent.Replace("{{totaldiscount}}", Convert.ToDouble(itemdiscount).ToString("F"));


            string consolidatetotal = "";
            string colscnt = "7";

            consolidatetotal = (total + taxamt).ToString();
            filecontent = filecontent.Replace("{{totalamount}}", consolidatetotal);
            double totalamt = (total + taxamt) - discount;
            string consolidatedtax = "";


            string str_consolidatetaxqry = @"select                                                 
                                                            isnull(STUFF((SELECT ',' + (select sub.code from tbl_taxmaster sub where  sub.id=tx.performa_wardtax)  + '@' + CONVERT(varchar,performa_wardtaxvalue) + '%' FROM sandd_tbl_performa_tax tx where tx.performa_wardid=it.performa_wardid and ISNULL(performa_wardtaxvalue,0)!=0 
                                                            FOR XML PATH (''), TYPE).value('.', 'varchar(max)'), 1, 1, ''),'') as taxname,
                                                            SUM(performa_wardtaxvalue)   as performa_wardtaxvalue                                              
                                                              FROM sandd_tbl_performa_tax it where ISNULL(performa_wardtaxvalue,0)!=0 and performa_wardid=@performa_wardid
                                                              group by performa_wardid";

            str_consolidatetaxqry = @"select(select sub.code from tbl_taxmaster sub where sub.id=it.performa_wardtax)  + '@' +  CONVERT(varchar,performa_wardtaxvalue)  as taxname,
                                    performa_wardtaxvalue   as performa_wardtaxvalue   FROM sandd_tbl_performa_tax it where ISNULL(performa_wardtaxvalue,0)!=0 and it.performa_wardid=@performa_wardid";

            string con_taxname = "";
            double con_taxvalue = 0;
            ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@performa_wardid", Convert.ToString(performa_wardid).Trim());
            dtsale = new DataTable();
            dtsale = ies.Fn_ExecutiveSql_Datatable(str_consolidatetaxqry, 0, 1);

            if (dtsale != null)
            {
                if (dtsale.Rows.Count > 0)
                {

                    for (int itaxid = 0; itaxid < dtsale.Rows.Count; itaxid++)
                    {

                        con_taxname = Convert.ToString(dtsale.Rows[itaxid]["taxname"]);
                        con_taxvalue = Convert.ToDouble(dtsale.Rows[itaxid]["performa_wardtaxvalue"]);

                        if (con_taxname != "")
                        {
                            double totalconsolidatetaxvalue = 0;
                            if (con_taxvalue != 0)
                            {
                                totalconsolidatetaxvalue = (Convert.ToDouble(total) - Convert.ToDouble(discount)) * Convert.ToDouble(Convert.ToDouble(con_taxvalue) / Convert.ToDouble(100));
                                totalamt = totalamt + totalconsolidatetaxvalue;

                            }

                            consolidatedtax += @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>" + con_taxname + @"%&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (totalconsolidatetaxvalue).ToString("F") + "</td></tr>";


                        }

                    }
                }
            }





            filecontent = filecontent.Replace("{{consolidatetax}}", consolidatedtax);

            string consolidatediscount = "";



            if (discount != 0)
            {

                consolidatediscount = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Discount&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (discount).ToString("F") + "</td></tr>";
            }

            filecontent = filecontent.Replace("{{overalldiscount}}", consolidatediscount);












            string roundoff = roundoff = (totalamt - Math.Round(totalamt, 0)).ToString("0.00");

            if (roundoff.IndexOf("-") == 0) roundoff = roundoff.Replace('-', '+');
            else roundoff = "-" + roundoff;

            string consolidateroundoff = "";
            if (roundoff != "" && roundoff != "-0.00" && roundoff != "0.00" && roundoff != "+0.00")
            {

                consolidateroundoff = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Rounded Off&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (roundoff) + "</td></tr>";
            }


            filecontent = filecontent.Replace("{{overallroundoff}}", consolidateroundoff);



            string consolidatenettotal = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Net Total&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (Math.Round(totalamt, 0)).ToString("F") + "</td></tr>";


            filecontent = filecontent.Replace("{{nettotal}}", consolidatenettotal);







            string consolidateShipmentCharge = "";


            if (Sale_shipmentcharge != 0)
            {

                consolidateShipmentCharge = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Shipment Charge&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (Sale_shipmentcharge).ToString("F") + "</td></tr>";

            }

            filecontent = filecontent.Replace("{{shipmentcharge}}", consolidateShipmentCharge);

            double grandamt = 0;


            grandamt = totalamt + Sale_shipmentcharge;

            string consolidategrandtotal = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Grand Total&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (Math.Round(grandamt, 0)).ToString("F") + "</td></tr>";


            filecontent = filecontent.Replace("{{grandtotal}}", consolidategrandtotal);




            string amtwords;
            string totalbillamt = (Math.Round(grandamt, 0)).ToString("F");
            IndiaCurrencyConverter con = new IndiaCurrencyConverter();
            if (IsNumeric(totalbillamt))
            {
                amtwords =
                (con.ConvertToWord(totalbillamt, System.Drawing.Color.Blue, "0") + " rupees only");
            }
            else
            {
                string amt = totalbillamt.ToString().Split('.')[0];
                string paise = Convert.ToDouble(totalbillamt).ToString("0.00").Split('.')[1];

                string totalamtinwords = "";
                string totalpaiseinwords = "";
                if (Convert.ToDouble(paise) == 0)
                {
                    totalamtinwords = con.ConvertToWord(amt, System.Drawing.Color.Blue, "1") + " rupees only";
                }
                else
                {
                    totalamtinwords = con.ConvertToWord(amt, System.Drawing.Color.Blue, "1") + " rupees and ";
                    totalpaiseinwords = con.ConvertToWord(paise, System.Drawing.Color.Blue, "1") + " paise only";
                }


                amtwords = (totalamtinwords + totalpaiseinwords);
            }

            filecontent = filecontent.Replace("{{amountinwords}}", amtwords);








            writer.Write(filecontent);

            writer.Close();
            //MessageBox.Show(filecontent);
            // web_report.DocumentText = filecontent;
            string strpath = Rootpath + "/Invoice";
            string domainname = "0001";
            string[] Inputfile = filelist.ToArray();
            PdfGenerator.Invoicepdf(strpath, domainname,
                Inputfile);

            //pdfViewer1.Document = new PdfDocument(@"D:\Chinna\IES\Development\Xsell_Retail\Xsell_Retail\Xsell_Retail\bin\Debug\001\0001.pdf");
            string file_pathfile = strpath + "/0001.pdf";
            //axAcroPDF1.LoadFile(file_pathfile);

        }
        catch
        {

        }

        // When a cursor is back to its default shape, it means the process ends

    }

    //-------------------------------Structures and classes----------------------------

    internal bool IsNumeric(string ValueInNumeric)
    {
        bool IsFine = true;
        foreach (char ch in ValueInNumeric)
        {
            if (!(ch >= '0' && ch <= '9'))
            {
                IsFine = false;
            }
        }
        return IsFine;
    }
}


public class Inward_DC_Pdf
{
    public Inward_DC_Pdf()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public Inward_DC_Pdf(string dc_id, string strticketno)
    {

        string discount_print_continuetext = @"<tr>
                    <td style='vertical-align: central; text-align: left; width: 7%' colspan='7'>
                        <div style='text-align: right'><b>Continue...</b></div>
                    </td>
                </tr>";

        string withdiscount_print_continuetext = @"<tr>
                    <td style='vertical-align: central; text-align: left; width: 7%' colspan='7'>
                        <div style='text-align: right'><b>Continue.......</b></div>
                    </td>
                </tr>";




        string sale_customername = "Unknown";
        string sale_quoteno = "";
        string sale_quotedate = "";
        string deliverymode = "";
        string dispatchdate = "";
        string buyer_tinno = "";

        string str_salesdetailqry = @"select 
                                        dc_id,dc_customername,
                                        dbo.UDF_DateFormat(dc_date,1,1) as dc_date,
                                        dbo.UDF_DateFormat(dc_date,1,0) as dispatchdate,
                                        isnull(dc_billdiscount,0) as dc_billdiscount,
                                        isnull(dc_shipmentcharge,0) as dc_shipmentcharge,                                        
                                        dc_ticketno as billNo,
                                        financialyear,dc_customerid,
                                        case when ISNULL(dc_deliverymode,'')='' then 'Hand Delivery' 	                                        
	                                        else ISNULL(dc_deliverymode,'Hand Delivery') end as deliverymode,
                                        case when ISNULL(dc_buyerorderno,'Verbal')='' then 'Verbal' else  ISNULL(dc_buyerorderno,'Verbal') end  as Verbal, 
                                        case when dbo.UDF_DateFormat(ISNULL(dc_buyerorderdate,'1900-01-01'),1,0)='01-01-1900'  then dbo.UDF_DateFormat(dc_date,1,0) else dbo.UDF_DateFormat(dc_buyerorderdate,1,0) end  as dc_quotedate
                                        ,isnull(orphan_id,'0') as orphan_id,isnull(dc_method,'1') as dc_method
                                         from sandd_tbl_sales_dc_workorder where dc_id=@dc_id";

        string billmethod = "";
        string dc_method = "1";
        string Terms = "";
        string sale_custid = "";
        string orphan_custid = "";
        double discount = 0.0;
        double Sale_shipmentcharge = 0.0;
        string strDate = "";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@dc_id", Convert.ToString(dc_id).Trim());
        DataTable dtsale = new DataTable();
        dtsale = ies.Fn_ExecutiveSql_Datatable(str_salesdetailqry, 0, 1);

        if (dtsale != null)
        {
            if (dtsale.Rows.Count > 0)
            {
                strticketno = Convert.ToString(dtsale.Rows[0]["billNo"]);
                billmethod = Convert.ToString("Delivery Challan");
                dc_method = Convert.ToString(dtsale.Rows[0]["dc_method"]);
                discount = Convert.ToDouble(Convert.ToString(dtsale.Rows[0]["dc_billdiscount"]));
                Sale_shipmentcharge = Convert.ToDouble(Convert.ToString(dtsale.Rows[0]["dc_shipmentcharge"]));
                strDate = Convert.ToString(dtsale.Rows[0]["dc_date"]);
                sale_customername = Convert.ToString(dtsale.Rows[0]["dc_customername"]);


                sale_custid = Convert.ToString(dtsale.Rows[0]["dc_customerid"]);
                orphan_custid = Convert.ToString(dtsale.Rows[0]["orphan_id"]);
                sale_quoteno = Convert.ToString(dtsale.Rows[0]["Verbal"]);
                sale_quotedate = Convert.ToString(dtsale.Rows[0]["dc_quotedate"]);
                deliverymode = Convert.ToString(dtsale.Rows[0]["deliverymode"]);
                dispatchdate = Convert.ToString(dtsale.Rows[0]["dispatchdate"]);
            }
        }



        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        string strquery = @"select top 1 * from (select distinct om.CompanyName as Company,cm.Mobile,cm.emailid,om.Area,District as[District],om.Pincode,cm.company_id,om.Address,
                                    (select st.Name from tbl_statemaster st where om.State=st.id) as State,companyno,isnull(om.TinNo,'') as TinNo,ISNULL(GSTIN,'') as GSTIN
                                    from tbl_Companycustomercontact cm inner join tbl_companycustomer om on om.company_id=cm.company_id ) T where 1=1";
        strquery += " and company_id='" + sale_custid + "'";
        DataTable dt_companycontact = ies.Fn_ExecutiveSql_Datatable(strquery, 0, 1);


        string str_qry =
        @"			SELECT
                ROW_NUMBER() over(order by dc_itemid) as slno,
                case when ISNULL(hsn_code,'')='' then '' else  ISNULL(hsn_code,'')  end  as HSN,
                it.dc_productname as Name,
				case when isnull(it.dc_productdesc,'')='NA' then '' else dc_productdesc end as dc_productdesc,
                dc_rate,
                dc_qty,
                convert(float,((dc_rate * dc_qty))) as total,
                dc_discount as dc_discount,
                ((convert(float,it.dc_rate) * convert(float,it.dc_qty)) - isnull(it.dc_discount,0)) as withouttaxamt,
                ((convert(float,it.dc_rate) * convert(float,it.dc_qty)) - isnull(it.dc_discount,0))
                    + 
                isnull((select ((convert(float,it.dc_rate) * convert(float,it.dc_qty)) - isnull(it.dc_discount,0))
                * (SUM(convert(float,dc_itemtaxvalue))/100) FROM sandd_tbl_sales_dc_workorder_items_tax tx where tx.dc_id=it.dc_id and tx.dc_itemid=it.dc_itemid),0) as totalrate,
                isnull((select ((convert(float,it.dc_rate) * convert(float,it.dc_qty)) - isnull(it.dc_discount,0))
                * (SUM(convert(float,tx.dc_itemtaxvalue))/100) FROM sandd_tbl_sales_dc_workorder_items_tax tx where tx.dc_id=it.dc_id and tx.dc_itemid=it.dc_itemid),0) as taxrate,
                isnull(STUFF((SELECT ',' + (select sub.code from tbl_taxmaster sub where  sub.id=tx.dc_itemtax)   + '@ ' + CONVERT(varchar,tx.dc_itemtaxvalue) + '%' FROM sandd_tbl_sales_dc_workorder_items_tax tx where tx.dc_id=it.dc_id and tx.dc_itemid=it.dc_itemid                  
                FOR XML PATH (''), TYPE).value('.', 'varchar(max)'), 1, 1, ''),'') as taxname
                  FROM sandd_tbl_sales_dc_workorder_items it where dc_id=@dc_id";

        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@dc_id", Convert.ToString(dc_id).Trim());
        DataTable dtitem = new DataTable();
        dtitem = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);

        var discountcount = dtitem.AsEnumerable().
        Count(row => row.Field<double>("dc_discount") != 0.0);

        string strbcData = strticketno;




        List<string> filelist = new List<string>();
        int totalfile = 0;
        string filefolder = HttpContext.Current.Server.MapPath("~/Invoice");
        if (System.IO.Directory.Exists(filefolder) == true)
        {
            DirectoryInfo dr = new DirectoryInfo(filefolder);
            foreach (var fi in dr.GetFiles())
            {
                File.Delete(fi.FullName);
            }
        }
        else
        {
            System.IO.Directory.CreateDirectory(filefolder);
        }



        string Rootpath = HttpContext.Current.Server.MapPath("~");

        string strpath_withdiscount = Rootpath + @"\Format\inward_dc_invoice_finalwithdiscount.html";
        string strpath_withoutdiscount = Rootpath + @"\Format\inward_dc_invoice_finalwithoutdiscount.html";
        string strpath_withoutrate = Rootpath + @"\Format\inward_dc_invoice_finalwithoutrate.html";

        string filecontent;


        //Print buying goods
        double total = 0.0;

        double taxamt = 0.0;
        double itemdiscount = 0.0;

        Int32 totalqty = 0;
        int i = 0;
        int currow = 0;


        double sale_Billamt = 0;
        double receivedamt = 0;
        string Sales_paymentmode = "";








    ContinueFinal:

        string filepath = filefolder + @"\" + "File_" + Convert.ToString(totalfile) + ".html";
        if (File.Exists(filepath))
        {
            File.Delete(filepath);
        }
        StreamWriter writer = new StreamWriter(filepath);
        filelist.Add(filepath);

        try
        {


            bool isdiscount = false;

            string continuetext = "";

            if (dc_method == "1")
            {

                if (discountcount != 0)
                {
                    var fileStream = new FileStream(strpath_withdiscount, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        filecontent = streamReader.ReadToEnd();
                    }

                    isdiscount = true;
                    continuetext = discount_print_continuetext;
                }
                else
                {
                    var fileStream = new FileStream(strpath_withoutdiscount, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        filecontent = streamReader.ReadToEnd();
                    }
                    continuetext = withdiscount_print_continuetext;
                }
            }
            else
            {
                var fileStream = new FileStream(strpath_withoutrate, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    filecontent = streamReader.ReadToEnd();
                }
                continuetext = discount_print_continuetext;
            }





            if (dt_companycontact != null)
            {
                if (dt_companycontact.Rows.Count > 0)
                {

                    buyer_tinno = Convert.ToString(dt_companycontact.Rows[0]["TinNo"]);
                    string buyer_GSTIN = Convert.ToString(dt_companycontact.Rows[0]["GSTIN"]);
                    filecontent = filecontent.Replace("{{companyname}}", Convert.ToString(dt_companycontact.Rows[0]["Company"]));


                    filecontent = filecontent.Replace("{{area}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));

                    filecontent = filecontent.Replace("{{state}}", Convert.ToString(dt_companycontact.Rows[0]["District"]) + "," + Convert.ToString(dt_companycontact.Rows[0]["state"]));
                    if (buyer_tinno != "" && buyer_GSTIN != "")
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".<br>" + "Buyer's Sales Tax No." + buyer_tinno + "<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                    }
                    else if (buyer_tinno == "" && buyer_GSTIN != "")
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                    }
                    else
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".");
                    }

                    filecontent = filecontent.Replace("{{address}}", Convert.ToString(dt_companycontact.Rows[0]["address"]));

                    filecontent = filecontent.Replace("{{customerrefno}}", Convert.ToString(dt_companycontact.Rows[0]["companyno"]));

                    filecontent = filecontent.Replace("{{otherref}}", "-");
                    filecontent = filecontent.Replace("{{deliverynote}}", "-");
                    filecontent = filecontent.Replace("{{dispatchto}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));



                }
                else
                {

                    ies = new IES_Generic_Function();
                    ies.ies_parameters.Clear();
                    strquery = @"select isnull(om.CompanyName,'') as Company,
                                    ISNULL(om.Mobileno,'') as Mobile,isnull(om.emailid,'') as emailid,
                                    isnull(om.Area,'') as Area,
                                    isnull(District,'') as[District],
                                    isnull(om.Pincode,'') as Pincode,'' as Address,
                                    '' as State,'' as companyno,
                                    isnull(om.TinNo,'') as TinNo,
                                    ISNULL(GSTIN,'') as GSTIN,
                                    '0' as company_id
                                    from tbl_orphancustomer om where 1=1 ";
                    strquery += " and company_id='" + orphan_custid + "'";
                    dt_companycontact = ies.Fn_ExecutiveSql_Datatable(strquery, 0, 1);


                    if (dt_companycontact != null)
                    {
                        if (dt_companycontact.Rows.Count > 0)
                        {

                            buyer_tinno = Convert.ToString(dt_companycontact.Rows[0]["TinNo"]);
                            string buyer_GSTIN = Convert.ToString(dt_companycontact.Rows[0]["GSTIN"]);
                            filecontent = filecontent.Replace("{{companyname}}", Convert.ToString(dt_companycontact.Rows[0]["Company"]));
                            if (Convert.ToString(dt_companycontact.Rows[0]["area"]) == "")
                            {
                                filecontent = filecontent.Replace("{{area}},", "");
                            }
                            else
                            {
                                filecontent = filecontent.Replace("{{area}},", Convert.ToString(dt_companycontact.Rows[0]["area"]) + ",");
                            }
                            if (Convert.ToString(dt_companycontact.Rows[0]["District"]) == "")
                            {
                                filecontent = filecontent.Replace("{{state}} - ", "");
                            }
                            else
                            {
                                filecontent = filecontent.Replace("{{state}} - ", Convert.ToString(dt_companycontact.Rows[0]["District"]) + ",");
                            }

                            if (buyer_tinno != "" && buyer_GSTIN != "")
                            {
                                filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + "<br>" + "Buyer's Sales Tax No." + buyer_tinno + "<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                            }
                            else if (buyer_tinno == "" && buyer_GSTIN != "")
                            {
                                filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + "<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                            }
                            else
                            {

                                if (Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) == "")
                                {
                                    filecontent = filecontent.Replace("{{postal}}", "");
                                }
                                else
                                {
                                    filecontent = filecontent.Replace("{{postal}}", "<br>" + Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".");
                                }

                            }
                            filecontent = filecontent.Replace("{{dispatchto}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));
                        }
                    }

                    filecontent = filecontent.Replace("{{companyname}}", sale_customername);
                    filecontent = filecontent.Replace("{{postal}}", "&nbsp;");
                    filecontent = filecontent.Replace("{{area}}", "&nbsp;");
                    filecontent = filecontent.Replace("{{area}},", "&nbsp;");
                    filecontent = filecontent.Replace("{{state}}", "&nbsp;");
                    filecontent = filecontent.Replace("<div>{{address}},</div>", "");
                    filecontent = filecontent.Replace("{{address}}", "&nbsp;");
                    filecontent = filecontent.Replace("{{address}},", "&nbsp;");
                    filecontent = filecontent.Replace("{{customerrefno}}", "-");
                    filecontent = filecontent.Replace("{{otherref}}", "-");
                    filecontent = filecontent.Replace("{{deliverynote}}", "-");
                    filecontent = filecontent.Replace("{{dispatchto}}", "-");
                }
            }
            else
            {
                filecontent = filecontent.Replace("{{companyname}}", sale_customername);

                filecontent = filecontent.Replace("<div>{{area}},</div>", "");
                filecontent = filecontent.Replace("<div>{{state}} - {{postal}}.</div>", "");
                filecontent = filecontent.Replace("<div>{{address}},</div>", "");


                filecontent = filecontent.Replace("{{postal}}", "&nbsp;");
                filecontent = filecontent.Replace("<div>{{area}},</div>", "");
                filecontent = filecontent.Replace("{{area}}", "&nbsp;");
                filecontent = filecontent.Replace("{{state}}", "&nbsp;");
                filecontent = filecontent.Replace("<div>{{address}},</div>", "");
                filecontent = filecontent.Replace("{{address}}", "&nbsp;");
                filecontent = filecontent.Replace("{{customerrefno}}", "-");
                filecontent = filecontent.Replace("{{otherref}}", "-");
                filecontent = filecontent.Replace("{{deliverynote}}", "-");
                filecontent = filecontent.Replace("{{dispatchto}}", "-");
            }


            filecontent = filecontent.Replace("{{invoiceno}}", strticketno);
            filecontent = filecontent.Replace("{{invoicedate}}", strDate);
            filecontent = filecontent.Replace("{{deliverynote}}", "-");
            filecontent = filecontent.Replace("{{buyerorderno}}", sale_quoteno);
            filecontent = filecontent.Replace("{{buyerordernodate}}", sale_quotedate);
            filecontent = filecontent.Replace("{{dispatchdocno}}", "-");
            filecontent = filecontent.Replace("{{dispatchthrough}}", deliverymode);
            filecontent = filecontent.Replace("{{dispatchdate}}", dispatchdate);

            filecontent = filecontent.Replace("{{modeofpayment}}", Sales_paymentmode);


            int initialrow = 0;

            string item_details = string.Empty;

            if (dtitem != null)
            {


                for (i = currow; i < dtitem.Rows.Count; i++)
                {

                    int checkrow = 15;

                    if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                    {
                        checkrow = 12;
                    }

                    initialrow += 1;
                    currow = i;
                    string verticalcenter = "center";
                    string str_trstyle = " style='vertical-align:" + verticalcenter + ";height:30px'";
                    if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                    {
                        str_trstyle = " style='vertical-align: top;height:60px'";
                        verticalcenter = "top";
                    }
                    if (dtitem.Rows.Count - 1 == i)
                    {
                        int totallength = 15 - initialrow;

                        if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                        {
                            totallength = 12 - initialrow;
                        }


                        if (totallength > 0)
                        {
                            verticalcenter = "top";
                            string t_length = (totallength * 25).ToString() + "px";
                            str_trstyle = " style='vertical-align:" + verticalcenter + ";height:" + t_length + @"'";

                            if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                            {
                                //str_trstyle = " style='vertical-align: " + verticalcenter + ";height:60px'";
                            }
                        }
                    }

                    string description = Convert.ToString(dtitem.Rows[i]["Name"]);


                    string HSN = Convert.ToString(dtitem.Rows[i]["HSN"]);
                    string Specification = Convert.ToString(dtitem.Rows[i]["dc_productdesc"]); ;
                    string SpecificationInfo = "";
                    if (Specification != "")
                    {
                        description += "<br /><b>" + Specification + "</b>";
                    }
                    if (SpecificationInfo != "")
                    {
                        description += "<br /><b>" + SpecificationInfo + "</b>";
                    }

                    string sale_qty = Convert.ToString(dtitem.Rows[i]["dc_qty"]);
                    string sale_rate = Convert.ToDouble(dtitem.Rows[i]["dc_rate"]).ToString("F");
                    string sale_discount = Convert.ToDouble(dtitem.Rows[i]["dc_discount"]).ToString("F");
                    string withouttaxamt = Convert.ToDouble(dtitem.Rows[i]["withouttaxamt"]).ToString("F");
                    string taxname = Convert.ToString(dtitem.Rows[i]["taxname"]);
                    string taxrate = Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["taxrate"])).ToString("F");

                    if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                    {
                        description += "<br /><div style='text-align:right!important'>" + taxname + "</div>";
                        sale_rate += "<br /><div style='text-align:right!important'>&nbsp;</div>";
                        withouttaxamt += "<br /><div style='text-align:right!important'>" + taxrate + "</div>";
                    }

                    if (dc_method == "1")
                    {

                        if (isdiscount == true)
                        {
                            if (item_details == string.Empty)
                            {

                                item_details = @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align: " + verticalcenter + ";text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='2' style='vertical-align:" + verticalcenter + ";'>" + description + @"</td>                    
                                                         <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                            
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_discount + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                            }
                            else
                            {
                                item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='2' style='vertical-align:" + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                            
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_discount + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                            }
                        }
                        else
                        {
                            if (item_details == string.Empty)
                            {

                                item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                                                                                    
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                            }
                            else
                            {
                                item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                                                                                    
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                            }
                        }
                    }
                    else
                    {

                        if (item_details == string.Empty)
                        {

                            item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td colspan='2' style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td colspan='2' style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        </tr>";
                        }
                        else
                        {
                            item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td colspan='2' style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td colspan='2' style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        </tr>";
                        }
                    }

                    total += Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["withouttaxamt"]));
                    totalqty += Convert.ToInt32(Convert.ToString(dtitem.Rows[i]["dc_qty"]));
                    taxamt += Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["taxrate"]));
                    itemdiscount += Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["dc_discount"]));

                    if (initialrow == checkrow)
                    {
                        filecontent = filecontent.Replace("{{rowdata}}", item_details);
                        filecontent = filecontent.Replace("{{modeofpayment}}", Sales_paymentmode);





                        writer.Write(filecontent);
                        writer.Write(continuetext);

                        //writer.Write("</table>This is a Computer Generated Delivery Challan</div></div></body></html>");
                        writer.Close();
                        totalfile += 1;
                        initialrow = 0;
                        currow += 1;
                        goto ContinueFinal;
                    }

                }
            }



            filecontent = filecontent.Replace("{{rowdata}}", item_details);

            writer.Write(filecontent);
            filecontent = "";

            strpath_withdiscount = Rootpath + @"\Format\inward_dc_invoice_discount.html";
            strpath_withoutdiscount = Rootpath + @"\Format\inward_dc_invoice_withoutdiscount.html";
            strpath_withoutrate = Rootpath + @"\Format\inward_dc_invoice_withoutrate.html";




            isdiscount = false;

            if (dc_method == "1")
            {

                if (discountcount != 0)
                {
                    var fileStream = new FileStream(strpath_withdiscount, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        filecontent = streamReader.ReadToEnd();
                    }

                    isdiscount = true;

                }
                else
                {
                    var fileStream = new FileStream(strpath_withoutdiscount, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        filecontent = streamReader.ReadToEnd();
                    }

                }
            }
            else
            {
                var fileStream = new FileStream(strpath_withoutrate, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    filecontent = streamReader.ReadToEnd();
                }

            }



            filecontent = filecontent.Replace("{{Terms}}", Terms);
            filecontent = filecontent.Replace("{{totalrate}}", "&nbsp;");
            filecontent = filecontent.Replace("{{totalqty}}", totalqty.ToString());
            filecontent = filecontent.Replace("{{totaldiscount}}", Convert.ToDouble(itemdiscount).ToString("F"));


            string consolidatetotal = "";
            string colscnt = "7";

            consolidatetotal = (total + taxamt).ToString();
            filecontent = filecontent.Replace("{{totalamount}}", consolidatetotal);
            double totalamt = (total + taxamt) - discount;
            string consolidatedtax = "";


            string str_consolidatetaxqry = @"select  isnull(STUFF((SELECT ',' + (select sub.code from tbl_taxmaster sub where  sub.id=tx.dc_tax)  + '@' + CONVERT(varchar,dc_taxvalue) + '%' 
                    FROM sandd_tbl_sales_dc_workorder_tax tx where tx.dc_id=it.dc_id and ISNULL(dc_taxvalue,0)!=0 
                                                            FOR XML PATH (''), TYPE).value('.', 'varchar(max)'), 1, 1, ''),'') as taxname,
                                                            SUM(dc_taxvalue)   as dc_taxvalue                                              
                                                              FROM sandd_tbl_sales_dc_workorder_tax it where ISNULL(dc_taxvalue,0)!=0 and dc_id=@dc_id
                                                              group by dc_id";

            str_consolidatetaxqry = @"select(select sub.code from tbl_taxmaster sub where sub.id=it.dc_tax)  + '@' +  CONVERT(varchar,dc_taxvalue)  as taxname,
                                    dc_taxvalue   as dc_taxvalue   FROM sandd_tbl_sales_dc_workorder_tax it where ISNULL(dc_taxvalue,0)!=0 and it.dc_id=@dc_id";

            string con_taxname = "";
            double con_taxvalue = 0;
            ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@dc_id", Convert.ToString(dc_id).Trim());
            dtsale = new DataTable();
            dtsale = ies.Fn_ExecutiveSql_Datatable(str_consolidatetaxqry, 0, 1);

            if (dtsale != null)
            {
                if (dtsale.Rows.Count > 0)
                {

                    for (int itaxid = 0; itaxid < dtsale.Rows.Count; itaxid++)
                    {

                        con_taxname = Convert.ToString(dtsale.Rows[itaxid]["taxname"]);
                        con_taxvalue = Convert.ToDouble(dtsale.Rows[itaxid]["dc_taxvalue"]);

                        if (con_taxname != "")
                        {
                            double totalconsolidatetaxvalue = 0;
                            if (con_taxvalue != 0)
                            {
                                totalconsolidatetaxvalue = (Convert.ToDouble(total) - Convert.ToDouble(discount)) * Convert.ToDouble(Convert.ToDouble(con_taxvalue) / Convert.ToDouble(100));
                                totalamt = totalamt + totalconsolidatetaxvalue;

                            }

                            consolidatedtax += @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>" + con_taxname + @"%&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (totalconsolidatetaxvalue).ToString("F") + "</td></tr>";


                        }

                    }
                }
            }





            filecontent = filecontent.Replace("{{consolidatetax}}", consolidatedtax);

            string consolidatediscount = "";



            if (discount != 0)
            {

                consolidatediscount = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Discount&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (discount).ToString("F") + "</td></tr>";
            }

            filecontent = filecontent.Replace("{{overalldiscount}}", consolidatediscount);












            string roundoff = roundoff = (totalamt - Math.Round(totalamt, 0)).ToString("0.00");

            if (roundoff.IndexOf("-") == 0) roundoff = roundoff.Replace('-', '+');
            else roundoff = "-" + roundoff;

            string consolidateroundoff = "";
            if (roundoff != "" && roundoff != "-0.00" && roundoff != "0.00" && roundoff != "+0.00")
            {

                consolidateroundoff = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Rounded Off&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (roundoff) + "</td></tr>";
            }


            filecontent = filecontent.Replace("{{overallroundoff}}", consolidateroundoff);



            string consolidatenettotal = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Net Total&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (Math.Round(totalamt, 0)).ToString("F") + "</td></tr>";


            filecontent = filecontent.Replace("{{nettotal}}", consolidatenettotal);







            string consolidateShipmentCharge = "";


            if (Sale_shipmentcharge != 0)
            {

                consolidateShipmentCharge = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Shipment Charge&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (Sale_shipmentcharge).ToString("F") + "</td></tr>";

            }

            filecontent = filecontent.Replace("{{shipmentcharge}}", consolidateShipmentCharge);

            double grandamt = 0;


            grandamt = totalamt + Sale_shipmentcharge;

            string consolidategrandtotal = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Grand Total&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (Math.Round(grandamt, 0)).ToString("F") + "</td></tr>";


            filecontent = filecontent.Replace("{{grandtotal}}", consolidategrandtotal);




            string amtwords;
            string totalbillamt = (Math.Round(grandamt, 0)).ToString("F");
            IndiaCurrencyConverter con = new IndiaCurrencyConverter();
            if (IsNumeric(totalbillamt))
            {
                amtwords =
                (con.ConvertToWord(totalbillamt, System.Drawing.Color.Blue, "0") + " rupees only");
            }
            else
            {
                string amt = totalbillamt.ToString().Split('.')[0];
                string paise = Convert.ToDouble(totalbillamt).ToString("0.00").Split('.')[1];

                string totalamtinwords = "";
                string totalpaiseinwords = "";
                if (Convert.ToDouble(paise) == 0)
                {
                    totalamtinwords = con.ConvertToWord(amt, System.Drawing.Color.Blue, "1") + " rupees only";
                }
                else
                {
                    totalamtinwords = con.ConvertToWord(amt, System.Drawing.Color.Blue, "1") + " rupees and ";
                    totalpaiseinwords = con.ConvertToWord(paise, System.Drawing.Color.Blue, "1") + " paise only";
                }


                amtwords = (totalamtinwords + totalpaiseinwords);
            }

            filecontent = filecontent.Replace("{{amountinwords}}", amtwords);







            writer.Write(filecontent);
            // writer.Write("</table>This is a Computer Generated Delivery Challan</div></div></body></html>");
            writer.Close();
            //MessageBox.Show(filecontent);
            // web_report.DocumentText = filecontent;
            string strpath = Rootpath + "/Invoice";
            string domainname = "0001";
            string[] Inputfile = filelist.ToArray();
            PdfGenerator.Invoicepdf(strpath, domainname,
                Inputfile);

            //pdfViewer1.Document = new PdfDocument(@"D:\Chinna\IES\Development\Xsell_Retail\Xsell_Retail\Xsell_Retail\bin\Debug\001\0001.pdf");
            string file_pathfile = strpath + "/0001.pdf";
            //axAcroPDF1.LoadFile(file_pathfile);

        }
        catch
        {

        }

        // When a cursor is back to its default shape, it means the process ends

    }

    //-------------------------------Structures and classes----------------------------

    internal bool IsNumeric(string ValueInNumeric)
    {
        bool IsFine = true;
        foreach (char ch in ValueInNumeric)
        {
            if (!(ch >= '0' && ch <= '9'))
            {
                IsFine = false;
            }
        }
        return IsFine;
    }
}



public class Inward_DC_Pdf_invoice
{
    public Inward_DC_Pdf_invoice()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public Inward_DC_Pdf_invoice(string dc_id, string strticketno, string delv_id)
    {

        string discount_print_continuetext = @"<tr>
                    <td style='vertical-align: central; text-align: left; width: 7%' colspan='7'>
                        <div style='text-align: right'><b>Continue...</b></div>
                    </td>
                </tr>";

        string sale_customername = "Unknown";
        string sale_quoteno = "";
        string sale_quotedate = "";
        string deliverymode = "";
        string dispatchdate = "";
        string buyer_tinno = "";

        string str_salesdetailqry = @"select 
                                        dc_id,dc_customername,
                                        dbo.UDF_DateFormat(dc_date,1,1) as dc_date,
                                        dbo.UDF_DateFormat(dc_date,1,0) as dispatchdate,
                                        dc_ticketno as billNo,
                                        financialyear,dc_customerid,
                                        case when ISNULL(dc_deliverymode,'')='' then 'Hand Delivery' 	                                        
	                                     else ISNULL(dc_deliverymode,'Hand Delivery') end as deliverymode,
                                        case when ISNULL(dc_buyerorderno,'Verbal')='' then 'Verbal' else  ISNULL(dc_buyerorderno,'Verbal') end  as Verbal, 
                                        case when dbo.UDF_DateFormat(ISNULL(dc_buyerorderdate,'1900-01-01'),1,0)='01-01-1900'  then dbo.UDF_DateFormat(dc_date,1,0) else dbo.UDF_DateFormat(dc_buyerorderdate,1,0) end  as dc_quotedate
                                        ,isnull(orphan_id,'0') as orphan_id,isnull(dc_method,'1') as dc_method
                                         from sandd_tbl_sales_dc_workorder where dc_id=@dc_id";

        string billmethod = "";
        string dc_method = "1";
        string Terms = "";
        string sale_custid = "";
        string orphan_custid = "";
        double discount = 0.0;
        double Sale_shipmentcharge = 0.0;
        string strDate = "";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@dc_id", Convert.ToString(dc_id).Trim());
        DataTable dtsale = new DataTable();
        dtsale = ies.Fn_ExecutiveSql_Datatable(str_salesdetailqry, 0, 1);

        if (dtsale != null)
        {
            if (dtsale.Rows.Count > 0)
            {
                strticketno = Convert.ToString(dtsale.Rows[0]["billNo"]);
                billmethod = Convert.ToString("Delivery Challan");
                dc_method = Convert.ToString(dtsale.Rows[0]["dc_method"]);
                strDate = Convert.ToString(dtsale.Rows[0]["dc_date"]);
                sale_customername = Convert.ToString(dtsale.Rows[0]["dc_customername"]);
                sale_custid = Convert.ToString(dtsale.Rows[0]["dc_customerid"]);
                orphan_custid = Convert.ToString(dtsale.Rows[0]["orphan_id"]);
                sale_quoteno = Convert.ToString(dtsale.Rows[0]["Verbal"]);
                sale_quotedate = Convert.ToString(dtsale.Rows[0]["dc_quotedate"]);
                deliverymode = Convert.ToString(dtsale.Rows[0]["deliverymode"]);
                dispatchdate = Convert.ToString(dtsale.Rows[0]["dispatchdate"]);
            }
        }



        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        string strquery = @"select top 1 * from (select distinct om.CompanyName as Company,cm.Mobile,cm.emailid,om.Area,District as[District],om.Pincode,cm.company_id,om.Address,
                                    (select st.Name from tbl_statemaster st where om.State=st.id) as State,companyno,isnull(om.TinNo,'') as TinNo,ISNULL(GSTIN,'') as GSTIN
                                    from tbl_Companycustomercontact cm inner join tbl_companycustomer om on om.company_id=cm.company_id ) T where 1=1";
        strquery += " and company_id='" + sale_custid + "'";
        DataTable dt_companycontact = ies.Fn_ExecutiveSql_Datatable(strquery, 0, 1);



        strquery = @"select delv_id as delv_id,dc_id,delivery_reference_no,invoice_no,
                            dbo.UDF_DateFormat(po.invoice_date,1,0) as  invoice_date,	                       
                            (select upper(usr_sub.Firstname)  from tbl_usermaster usr_sub where  usr_sub.Usrid=po.delivered_by) as delivered_by                           
	                        from sandd_tbl_sales_dc_workorder_delivery po where po.dc_id=@dc_id and delv_id=@delv_id";
        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@dc_id", Convert.ToString(dc_id).Trim());
        ies.ies_parameters.Add("@delv_id", Convert.ToString(delv_id).Trim());
        DataTable dt_info2 = ies.Fn_ExecutiveSql_Datatable(strquery, 0, 1);

        string DCInvoiceno = "";
        string DCInvoiceDate = "";
        if (dt_info2 != null)
        {
            if (dt_info2.Rows.Count > 0)
            {
                DCInvoiceno = Convert.ToString(dt_info2.Rows[0]["invoice_no"]);
                DCInvoiceDate = Convert.ToString(dt_info2.Rows[0]["invoice_date"]);
            }
        }



        string str_qry =
        @"select iot.dc_productname , case when ISNULL(it.dc_description,'')='NA' then '' else 
                            ISNULL(it.dc_description,'') end as ItemDescription,iot.hsn_code as HSN,
                            it.deliver_qty as qty from sandd_tbl_sales_dc_workorder_delivery_items it inner join 
                            sandd_tbl_sales_dc_workorder_items iot on iot.dc_itemid=it.dc_itemid and it.dc_id=iot.dc_id where it.dc_id=@dc_id and it.delv_id=@delv_id";

        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@dc_id", Convert.ToString(dc_id).Trim());
        ies.ies_parameters.Add("@delv_id", Convert.ToString(delv_id).Trim());
        DataTable dtitem = new DataTable();
        dtitem = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);
        string strbcData = strticketno;

        List<string> filelist = new List<string>();
        int totalfile = 0;
        string filefolder = HttpContext.Current.Server.MapPath("~/Invoice");
        if (System.IO.Directory.Exists(filefolder) == true)
        {
            DirectoryInfo dr = new DirectoryInfo(filefolder);
            foreach (var fi in dr.GetFiles())
            {
                File.Delete(fi.FullName);
            }
        }
        else
        {
            System.IO.Directory.CreateDirectory(filefolder);
        }
        string Rootpath = HttpContext.Current.Server.MapPath("~");


        string strpath_withoutrate = Rootpath + @"\Format\inward_dc2_invoice_finalwithoutrate.html";

        string filecontent;
        Int32 totalqty = 0;
        int i = 0;
        int currow = 0;       
        string Sales_paymentmode = "";

    ContinueFinal:

        string filepath = filefolder + @"\" + "File_" + Convert.ToString(totalfile) + ".html";
        if (File.Exists(filepath))
        {
            File.Delete(filepath);
        }
        StreamWriter writer = new StreamWriter(filepath);
        filelist.Add(filepath);

        try
        {


            bool isdiscount = false;

            string continuetext = "";


            var fileStream = new FileStream(strpath_withoutrate, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                filecontent = streamReader.ReadToEnd();
            }
            continuetext = discount_print_continuetext;






            if (dt_companycontact != null)
            {
                if (dt_companycontact.Rows.Count > 0)
                {

                    buyer_tinno = Convert.ToString(dt_companycontact.Rows[0]["TinNo"]);
                    string buyer_GSTIN = Convert.ToString(dt_companycontact.Rows[0]["GSTIN"]);
                    filecontent = filecontent.Replace("{{companyname}}", Convert.ToString(dt_companycontact.Rows[0]["Company"]));


                    filecontent = filecontent.Replace("{{area}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));

                    filecontent = filecontent.Replace("{{state}}", Convert.ToString(dt_companycontact.Rows[0]["District"]) + "," + Convert.ToString(dt_companycontact.Rows[0]["state"]));
                    if (buyer_tinno != "" && buyer_GSTIN != "")
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".<br>" + "Buyer's Sales Tax No." + buyer_tinno + "<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                    }
                    else if (buyer_tinno == "" && buyer_GSTIN != "")
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                    }
                    else
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".");
                    }

                    filecontent = filecontent.Replace("{{address}}", Convert.ToString(dt_companycontact.Rows[0]["address"]));

                    filecontent = filecontent.Replace("{{customerrefno}}", Convert.ToString(dt_companycontact.Rows[0]["companyno"]));

                    filecontent = filecontent.Replace("{{otherref}}", "-");
                    filecontent = filecontent.Replace("{{deliverynote}}", "-");
                    filecontent = filecontent.Replace("{{dispatchto}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));



                }
                else
                {

                    ies = new IES_Generic_Function();
                    ies.ies_parameters.Clear();
                    strquery = @"select isnull(om.CompanyName,'') as Company,
                                    ISNULL(om.Mobileno,'') as Mobile,isnull(om.emailid,'') as emailid,
                                    isnull(om.Area,'') as Area,
                                    isnull(District,'') as[District],
                                    isnull(om.Pincode,'') as Pincode,'' as Address,
                                    '' as State,'' as companyno,
                                    isnull(om.TinNo,'') as TinNo,
                                    ISNULL(GSTIN,'') as GSTIN,
                                    '0' as company_id
                                    from tbl_orphancustomer om where 1=1 ";
                    strquery += " and company_id='" + orphan_custid + "'";
                    dt_companycontact = ies.Fn_ExecutiveSql_Datatable(strquery, 0, 1);


                    if (dt_companycontact != null)
                    {
                        if (dt_companycontact.Rows.Count > 0)
                        {

                            buyer_tinno = Convert.ToString(dt_companycontact.Rows[0]["TinNo"]);
                            string buyer_GSTIN = Convert.ToString(dt_companycontact.Rows[0]["GSTIN"]);
                            filecontent = filecontent.Replace("{{companyname}}", Convert.ToString(dt_companycontact.Rows[0]["Company"]));
                            if (Convert.ToString(dt_companycontact.Rows[0]["area"]) == "")
                            {
                                filecontent = filecontent.Replace("{{area}},", "");
                            }
                            else
                            {
                                filecontent = filecontent.Replace("{{area}},", Convert.ToString(dt_companycontact.Rows[0]["area"]) + ",");
                            }
                            if (Convert.ToString(dt_companycontact.Rows[0]["District"]) == "")
                            {
                                filecontent = filecontent.Replace("{{state}} - ", "");
                            }
                            else
                            {
                                filecontent = filecontent.Replace("{{state}} - ", Convert.ToString(dt_companycontact.Rows[0]["District"]) + ",");
                            }

                            if (buyer_tinno != "" && buyer_GSTIN != "")
                            {
                                filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + "<br>" + "Buyer's Sales Tax No." + buyer_tinno + "<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                            }
                            else if (buyer_tinno == "" && buyer_GSTIN != "")
                            {
                                filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + "<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                            }
                            else
                            {

                                if (Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) == "")
                                {
                                    filecontent = filecontent.Replace("{{postal}}", "");
                                }
                                else
                                {
                                    filecontent = filecontent.Replace("{{postal}}", "<br>" + Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".");
                                }

                            }
                            filecontent = filecontent.Replace("{{dispatchto}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));
                        }
                    }

                    filecontent = filecontent.Replace("{{companyname}}", sale_customername);
                    filecontent = filecontent.Replace("{{postal}}", "&nbsp;");
                    filecontent = filecontent.Replace("{{area}}", "&nbsp;");
                    filecontent = filecontent.Replace("{{area}},", "&nbsp;");
                    filecontent = filecontent.Replace("{{state}}", "&nbsp;");
                    filecontent = filecontent.Replace("<div>{{address}},</div>", "");
                    filecontent = filecontent.Replace("{{address}}", "&nbsp;");
                    filecontent = filecontent.Replace("{{address}},", "&nbsp;");
                    filecontent = filecontent.Replace("{{customerrefno}}", "-");
                    filecontent = filecontent.Replace("{{otherref}}", "-");
                    filecontent = filecontent.Replace("{{deliverynote}}", "-");
                    filecontent = filecontent.Replace("{{dispatchto}}", "-");
                }
            }
            else
            {
                filecontent = filecontent.Replace("{{companyname}}", sale_customername);

                filecontent = filecontent.Replace("<div>{{area}},</div>", "");
                filecontent = filecontent.Replace("<div>{{state}} - {{postal}}.</div>", "");
                filecontent = filecontent.Replace("<div>{{address}},</div>", "");


                filecontent = filecontent.Replace("{{postal}}", "&nbsp;");
                filecontent = filecontent.Replace("<div>{{area}},</div>", "");
                filecontent = filecontent.Replace("{{area}}", "&nbsp;");
                filecontent = filecontent.Replace("{{state}}", "&nbsp;");
                filecontent = filecontent.Replace("<div>{{address}},</div>", "");
                filecontent = filecontent.Replace("{{address}}", "&nbsp;");
                filecontent = filecontent.Replace("{{customerrefno}}", "-");
                filecontent = filecontent.Replace("{{otherref}}", "-");
                filecontent = filecontent.Replace("{{deliverynote}}", "-");
                filecontent = filecontent.Replace("{{dispatchto}}", "-");
            }


           


            filecontent = filecontent.Replace("{{invoiceno}}", strticketno);
            filecontent = filecontent.Replace("{{invoicedate}}", strDate);
            filecontent = filecontent.Replace("{{deliverynote}}", "-");
            filecontent = filecontent.Replace("{{buyerorderno}}", sale_quoteno);
            filecontent = filecontent.Replace("{{buyerordernodate}}", sale_quotedate);
            filecontent = filecontent.Replace("{{dispatchdocno}}", "-");
            filecontent = filecontent.Replace("{{dispatchthrough}}", deliverymode);
            filecontent = filecontent.Replace("{{dispatchdate}}", dispatchdate);

            filecontent = filecontent.Replace("{{modeofpayment}}", Sales_paymentmode);


            int initialrow = 0;

            string item_details = string.Empty;

            if (dtitem != null)
            {


                for (i = currow; i < dtitem.Rows.Count; i++)
                {

                    int checkrow = 15;

                   

                    initialrow += 1;
                    currow = i;
                    string verticalcenter = "center";
                    string str_trstyle = " style='vertical-align:" + verticalcenter + ";height:30px'";
                    
                    if (dtitem.Rows.Count - 1 == i)
                    {
                        int totallength = 15 - initialrow;

                        if (totallength > 0)
                        {
                            verticalcenter = "top";
                            string t_length = (totallength * 45).ToString() + "px";
                            str_trstyle = " style='vertical-align:" + verticalcenter + ";height:" + t_length + @"'";                           
                        }
                    }

                    string description = Convert.ToString(dtitem.Rows[i]["dc_productname"]);


                    string HSN = Convert.ToString(dtitem.Rows[i]["HSN"]);
                    string Specification = Convert.ToString(dtitem.Rows[i]["ItemDescription"]); ;
                    string SpecificationInfo = "";
                    if (Specification != "")
                    {
                        description += "<br /><b>" + Specification + "</b>";
                    }
                    if (SpecificationInfo != "")
                    {
                        description += "<br /><b>" + SpecificationInfo + "</b>";
                    }

                    string sale_qty = Convert.ToString(dtitem.Rows[i]["qty"]);
                 


                    if (item_details == string.Empty)
                    {

                        item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td colspan='2' style='vertical-align:" + verticalcenter + "; text-align: center'>" + HSN + @"</td>
                                                        <td colspan='2' style='vertical-align: " + verticalcenter + "; text-align: center'>" + sale_qty + @"</td>
                                                        </tr>";
                    }
                    else
                    {
                        item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td colspan='2' style='vertical-align:" + verticalcenter + "; text-align: center'>" + HSN + @"</td>
                                                        <td colspan='2' style='vertical-align: " + verticalcenter + "; text-align: center'>" + sale_qty + @"</td>
                                                        </tr>";
                    }



                    totalqty += Convert.ToInt32(Convert.ToString(dtitem.Rows[i]["qty"]));
                 

                    if (initialrow == checkrow)
                    {
                        filecontent = filecontent.Replace("{{rowdata}}", item_details);
                        filecontent = filecontent.Replace("{{modeofpayment}}", Sales_paymentmode);

                        writer.Write(filecontent);
                        writer.Write(continuetext);

                        //writer.Write("</table>This is a Computer Generated Delivery Challan</div></div></body></html>");
                        writer.Close();
                        totalfile += 1;
                        initialrow = 0;
                        currow += 1;
                        goto ContinueFinal;
                    }

                }
            }



            filecontent = filecontent.Replace("{{rowdata}}", item_details);

            writer.Write(filecontent);
            filecontent = "";


            strpath_withoutrate = Rootpath + @"\Format\inward_dc2_invoice_withoutrate.html";




            isdiscount = false;


            fileStream = new FileStream(strpath_withoutrate, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                filecontent = streamReader.ReadToEnd();
            }





            filecontent = filecontent.Replace("{{Terms}}", Terms);
            filecontent = filecontent.Replace("{{totalrate}}", "&nbsp;");
            filecontent = filecontent.Replace("{{totalqty}}", totalqty.ToString());

            filecontent = filecontent.Replace("{{DCinvoiceno}}", DCInvoiceno);
            filecontent = filecontent.Replace("{{DCinvoicedate}}", DCInvoiceDate);

         
            string consolidatetotal = "";
            filecontent = filecontent.Replace("{{totalamount}}", consolidatetotal);     

            writer.Write(filecontent);
            // writer.Write("</table>This is a Computer Generated Delivery Challan</div></div></body></html>");
            writer.Close();
            //MessageBox.Show(filecontent);
            // web_report.DocumentText = filecontent;
            string strpath = Rootpath + "/Invoice";
            string domainname = "0001";
            string[] Inputfile = filelist.ToArray();
            PdfGenerator.Invoicepdf(strpath, domainname,
                Inputfile);

            //pdfViewer1.Document = new PdfDocument(@"D:\Chinna\IES\Development\Xsell_Retail\Xsell_Retail\Xsell_Retail\bin\Debug\001\0001.pdf");
            string file_pathfile = strpath + "/0001.pdf";
            //axAcroPDF1.LoadFile(file_pathfile);

        }
        catch
        {

        }

        // When a cursor is back to its default shape, it means the process ends

    }

    //-------------------------------Structures and classes----------------------------

    internal bool IsNumeric(string ValueInNumeric)
    {
        bool IsFine = true;
        foreach (char ch in ValueInNumeric)
        {
            if (!(ch >= '0' && ch <= '9'))
            {
                IsFine = false;
            }
        }
        return IsFine;
    }
}


///////////////////



/// <summary>
/// Summary description for Invoice_Pdf
/// </summary>
public class DC_HalfPage_Pdf
{
    public DC_HalfPage_Pdf()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public DC_HalfPage_Pdf(string dc_id, string strticketno)
    {

        string discount_print_continuetext = @"<tr>
                    <td style='vertical-align: central; text-align: left; width: 7%' colspan='7'>
                        <div style='text-align: right'><b>Continue...</b></div>
                    </td>
                </tr>";

        string withdiscount_print_continuetext = @"<tr>
                    <td style='vertical-align: central; text-align: left; width: 7%' colspan='7'>
                        <div style='text-align: right'><b>Continue.......</b></div>
                    </td>
                </tr>";




        string sale_customername = "Unknown";
        string sale_quoteno = "";
        string sale_quotedate = "";
        string deliverymode = "";
        string dispatchdate = "";
        string buyer_tinno = "";

        string str_salesdetailqry = @"select 
                                        dc_id,dc_customername,
                                        dbo.UDF_DateFormat(dc_date,1,1) as dc_date,
                                        dbo.UDF_DateFormat(dc_date,1,0) as dispatchdate,
                                        isnull(dc_billdiscount,0) as dc_billdiscount,
                                        isnull(dc_shipmentcharge,0) as dc_shipmentcharge,                                        
                                        dc_ticketno as billNo,
                                        financialyear,dc_customerid,
                                        case when ISNULL(dc_deliverymode,'')='' then 'Hand Delivery' 	                                        
	                                        else ISNULL(dc_deliverymode,'Hand Delivery') end as deliverymode,
                                        case when ISNULL(dc_buyerorderno,'Verbal')='' then 'Verbal' else  ISNULL(dc_buyerorderno,'Verbal') end  as Verbal, 
                                        case when dbo.UDF_DateFormat(ISNULL(dc_buyerorderdate,'1900-01-01'),1,0)='01-01-1900'  then dbo.UDF_DateFormat(dc_date,1,0) else dbo.UDF_DateFormat(dc_buyerorderdate,1,0) end  as dc_quotedate
                                        ,isnull(orphan_id,'0') as orphan_id,isnull(dc_method,'1') as dc_method,isnull(point1,'') as point1,isnull(point2,'') as point2,isnull(point3,'') as point3
                                         from sandd_tbl_sales_dc where dc_id=@dc_id";

        string billmethod = "";
        string dc_method = "1";
        string Terms = "";
        string sale_custid = "";
        string orphan_custid = "";
        double discount = 0.0;
        double Sale_shipmentcharge = 0.0;
        string strDate = "";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@dc_id", Convert.ToString(dc_id).Trim());
        DataTable dtsale = new DataTable();
        dtsale = ies.Fn_ExecutiveSql_Datatable(str_salesdetailqry, 0, 1);

        if (dtsale != null)
        {
            if (dtsale.Rows.Count > 0)
            {
                strticketno = Convert.ToString(dtsale.Rows[0]["billNo"]);
                billmethod = Convert.ToString("Delivery Challan");
                dc_method = Convert.ToString(dtsale.Rows[0]["dc_method"]);
                discount = Convert.ToDouble(Convert.ToString(dtsale.Rows[0]["dc_billdiscount"]));
                Sale_shipmentcharge = Convert.ToDouble(Convert.ToString(dtsale.Rows[0]["dc_shipmentcharge"]));
                strDate = Convert.ToString(dtsale.Rows[0]["dc_date"]);
                sale_customername = Convert.ToString(dtsale.Rows[0]["dc_customername"]);
                Terms = "<ul>";
                if (Convert.ToString(dtsale.Rows[0]["point1"]).Trim() != "")
                {
                    Terms += "<li>" + Convert.ToString(dtsale.Rows[0]["point1"]) + "</li>";
                }
                if (Convert.ToString(dtsale.Rows[0]["point2"]).Trim() != "")
                {
                    Terms += "<li>" + Convert.ToString(dtsale.Rows[0]["point2"]) + "</li>";
                }
                if (Convert.ToString(dtsale.Rows[0]["point3"]).Trim() != "")
                {
                    Terms += "<li>" + Convert.ToString(dtsale.Rows[0]["point3"]) + "</li>";
                }
                Terms += "</ul>";

                sale_custid = Convert.ToString(dtsale.Rows[0]["dc_customerid"]);
                orphan_custid = Convert.ToString(dtsale.Rows[0]["orphan_id"]);
                sale_quoteno = Convert.ToString(dtsale.Rows[0]["Verbal"]);
                sale_quotedate = Convert.ToString(dtsale.Rows[0]["dc_quotedate"]);
                deliverymode = Convert.ToString(dtsale.Rows[0]["deliverymode"]);
                dispatchdate = Convert.ToString(dtsale.Rows[0]["dispatchdate"]);
            }
        }



        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        string strquery = @"select top 1 * from (select distinct om.CompanyName as Company,cm.Mobile,cm.emailid,om.Area,District as[District],om.Pincode,cm.company_id,om.Address,
                                    (select st.Name from tbl_statemaster st where om.State=st.id) as State,companyno,isnull(om.TinNo,'') as TinNo,ISNULL(GSTIN,'') as GSTIN
                                    from tbl_Companycustomercontact cm inner join tbl_companycustomer om on om.company_id=cm.company_id ) T where 1=1";
        strquery += " and company_id='" + sale_custid + "'";
        DataTable dt_companycontact = ies.Fn_ExecutiveSql_Datatable(strquery, 0, 1);


        string str_qry =
        @"			SELECT
                ROW_NUMBER() over(order by dc_itemid) as slno,
                case when ISNULL(hsn_code,'')='' then 
                case when dc_producttype=1 then (select isnull(pt.hsn_code,'') from store_component pt where Componentid=dc_productcode)
                when dc_producttype=2 then (select isnull(s.hsn_code,'')  from store_Product s where Product_id=dc_productcode)  else '' end
                else  ISNULL(hsn_code,'')  end  as HSN,
                case 
                when dc_producttype=1 then
                (
                select 
                isnull((select cn.ShortName from store_ComponentName cn where cn.ComponentID=pt.ComponentName),'NA')                                
                + '-' + isnull((select cn.ShortName from store_Packagetype cn where cn.PackageID=pt.Packagetype),'NA')    
                + '-' + isnull((select cn.PrimaryValue from store_PrimaryValue cn where cn.PrimaryID=pt.ComponentValue),'NA')     
                from store_component pt 
                 where Componentid=dc_productcode)
                when dc_producttype=2 then
                (select s.Name from store_Product s where Product_id=dc_productcode)                
                end as Name,
                dc_rate,
                dc_qty,
                convert(float,((dc_rate * dc_qty))) as total,
                dc_discount as dc_discount,
                ((convert(float,it.dc_rate) * convert(float,it.dc_qty)) - isnull(it.dc_discount,0)) as withouttaxamt,
                ((convert(float,it.dc_rate) * convert(float,it.dc_qty)) - isnull(it.dc_discount,0))
                    + 
                isnull((select ((convert(float,it.dc_rate) * convert(float,it.dc_qty)) - isnull(it.dc_discount,0))
                * (SUM(convert(float,dc_itemtaxvalue))/100) FROM sandd_tbl_sales_dc_items_tax tx where tx.dc_id=it.dc_id and tx.dc_itemid=it.dc_itemid),0) as totalrate,
                isnull((select ((convert(float,it.dc_rate) * convert(float,it.dc_qty)) - isnull(it.dc_discount,0))
                * (SUM(convert(float,tx.dc_itemtaxvalue))/100) FROM sandd_tbl_sales_dc_items_tax tx where tx.dc_id=it.dc_id and tx.dc_itemid=it.dc_itemid),0) as taxrate,
                isnull(STUFF((SELECT ',' + (select sub.code from tbl_taxmaster sub where  sub.id=tx.dc_itemtax)   + '@ ' + CONVERT(varchar,tx.dc_itemtaxvalue) + '%' FROM sandd_tbl_sales_dc_items_tax tx where tx.dc_id=it.dc_id and tx.dc_itemid=it.dc_itemid                  
                FOR XML PATH (''), TYPE).value('.', 'varchar(max)'), 1, 1, ''),'') as taxname,
                case when isnull(dc_serialno,'')='' then '' else isnull(dc_serialno,'') end as dc_serialno
                  FROM sandd_tbl_sales_dc_items it where dc_id=@dc_id";

        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@dc_id", Convert.ToString(dc_id).Trim());
        DataTable dtitem = new DataTable();
        dtitem = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);

        var discountcount = dtitem.AsEnumerable().
        Count(row => row.Field<double>("dc_discount") != 0.0);

        string strbcData = strticketno;




        List<string> filelist = new List<string>();
        int totalfile = 0;
        string filefolder = HttpContext.Current.Server.MapPath("~/Invoice");
        if (System.IO.Directory.Exists(filefolder) == true)
        {
            DirectoryInfo dr = new DirectoryInfo(filefolder);
            foreach (var fi in dr.GetFiles())
            {
                File.Delete(fi.FullName);
            }
        }
        else
        {
            System.IO.Directory.CreateDirectory(filefolder);
        }



        string Rootpath = HttpContext.Current.Server.MapPath("~");

        string strpath_withdiscount = Rootpath + @"\Format\dc_halfpage_invoice_finalwithdiscount.html";
        string strpath_withoutdiscount = Rootpath + @"\Format\dc_halfpage_invoice_finalwithoutdiscount.html";
        string strpath_withoutrate = Rootpath + @"\Format\dc_halfpage_invoice_finalwithoutrate.html";

        string filecontent;


        //Print buying goods
        double total = 0.0;

        double taxamt = 0.0;
        double itemdiscount = 0.0;

        Int32 totalqty = 0;
        int i = 0;
        int currow = 0;


        double sale_Billamt = 0;
        double receivedamt = 0;
        string Sales_paymentmode = "";








    ContinueFinal:

        string filepath = filefolder + @"\" + "File_" + Convert.ToString(totalfile) + ".html";
        if (File.Exists(filepath))
        {
            File.Delete(filepath);
        }
        StreamWriter writer = new StreamWriter(filepath);
        filelist.Add(filepath);

        try
        {


            bool isdiscount = false;

            string continuetext = "";

            if (dc_method == "1")
            {

                if (discountcount != 0)
                {
                    var fileStream = new FileStream(strpath_withdiscount, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        filecontent = streamReader.ReadToEnd();
                    }

                    isdiscount = true;
                    continuetext = discount_print_continuetext;
                }
                else
                {
                    var fileStream = new FileStream(strpath_withoutdiscount, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        filecontent = streamReader.ReadToEnd();
                    }
                    continuetext = withdiscount_print_continuetext;
                }
            }
            else
            {
                var fileStream = new FileStream(strpath_withoutrate, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    filecontent = streamReader.ReadToEnd();
                }
                continuetext = discount_print_continuetext;
            }





            if (dt_companycontact != null)
            {
                if (dt_companycontact.Rows.Count > 0)
                {

                    buyer_tinno = Convert.ToString(dt_companycontact.Rows[0]["TinNo"]);
                    string buyer_GSTIN = Convert.ToString(dt_companycontact.Rows[0]["GSTIN"]);
                    filecontent = filecontent.Replace("{{companyname}}", Convert.ToString(dt_companycontact.Rows[0]["Company"]));


                    filecontent = filecontent.Replace("{{area}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));

                    filecontent = filecontent.Replace("{{state}}", Convert.ToString(dt_companycontact.Rows[0]["District"]) + "," + Convert.ToString(dt_companycontact.Rows[0]["state"]));
                    if (buyer_tinno != "" && buyer_GSTIN != "")
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".<br>" + "Buyer's Sales Tax No." + buyer_tinno + "<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                    }
                    else if (buyer_tinno == "" && buyer_GSTIN != "")
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                    }
                    else
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".");
                    }

                    filecontent = filecontent.Replace("{{address}}", Convert.ToString(dt_companycontact.Rows[0]["address"]));

                    filecontent = filecontent.Replace("{{customerrefno}}", Convert.ToString(dt_companycontact.Rows[0]["companyno"]));

                    filecontent = filecontent.Replace("{{otherref}}", "-");
                    filecontent = filecontent.Replace("{{deliverynote}}", "-");
                    filecontent = filecontent.Replace("{{dispatchto}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));



                }
                else
                {

                    ies = new IES_Generic_Function();
                    ies.ies_parameters.Clear();
                    strquery = @"select isnull(om.CompanyName,'') as Company,
                                    ISNULL(om.Mobileno,'') as Mobile,isnull(om.emailid,'') as emailid,
                                    isnull(om.Area,'') as Area,
                                    isnull(District,'') as[District],
                                    isnull(om.Pincode,'') as Pincode,'' as Address,
                                    '' as State,'' as companyno,
                                    isnull(om.TinNo,'') as TinNo,
                                    ISNULL(GSTIN,'') as GSTIN,
                                    '0' as company_id
                                    from tbl_orphancustomer om where 1=1 ";
                    strquery += " and company_id='" + orphan_custid + "'";
                    dt_companycontact = ies.Fn_ExecutiveSql_Datatable(strquery, 0, 1);


                    if (dt_companycontact != null)
                    {
                        if (dt_companycontact.Rows.Count > 0)
                        {

                            buyer_tinno = Convert.ToString(dt_companycontact.Rows[0]["TinNo"]);
                            string buyer_GSTIN = Convert.ToString(dt_companycontact.Rows[0]["GSTIN"]);
                            filecontent = filecontent.Replace("{{companyname}}", Convert.ToString(dt_companycontact.Rows[0]["Company"]));
                            if (Convert.ToString(dt_companycontact.Rows[0]["area"]) == "")
                            {
                                filecontent = filecontent.Replace("{{area}},", "");
                            }
                            else
                            {
                                filecontent = filecontent.Replace("{{area}},", Convert.ToString(dt_companycontact.Rows[0]["area"]) + ",");
                            }
                            if (Convert.ToString(dt_companycontact.Rows[0]["District"]) == "")
                            {
                                filecontent = filecontent.Replace("{{state}} - ", "");
                            }
                            else
                            {
                                filecontent = filecontent.Replace("{{state}} - ", Convert.ToString(dt_companycontact.Rows[0]["District"]) + ",");
                            }

                            if (buyer_tinno != "" && buyer_GSTIN != "")
                            {
                                filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + "<br>" + "Buyer's Sales Tax No." + buyer_tinno + "<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                            }
                            else if (buyer_tinno == "" && buyer_GSTIN != "")
                            {
                                filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + "<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                            }
                            else
                            {

                                if (Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) == "")
                                {
                                    filecontent = filecontent.Replace("{{postal}}", "");
                                }
                                else
                                {
                                    filecontent = filecontent.Replace("{{postal}}", "<br>" + Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".");
                                }

                            }
                            filecontent = filecontent.Replace("{{dispatchto}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));
                        }
                    }

                    filecontent = filecontent.Replace("{{companyname}}", sale_customername);
                    filecontent = filecontent.Replace("{{postal}}", "&nbsp;");
                    filecontent = filecontent.Replace("{{area}}", "&nbsp;");
                    filecontent = filecontent.Replace("{{area}},", "&nbsp;");
                    filecontent = filecontent.Replace("{{state}}", "&nbsp;");
                    filecontent = filecontent.Replace("<div>{{address}},</div>", "");
                    filecontent = filecontent.Replace("{{address}}", "&nbsp;");
                    filecontent = filecontent.Replace("{{address}},", "&nbsp;");
                    filecontent = filecontent.Replace("{{customerrefno}}", "-");
                    filecontent = filecontent.Replace("{{otherref}}", "-");
                    filecontent = filecontent.Replace("{{deliverynote}}", "-");
                    filecontent = filecontent.Replace("{{dispatchto}}", "-");
                }
            }
            else
            {
                filecontent = filecontent.Replace("{{companyname}}", sale_customername);

                filecontent = filecontent.Replace("<div>{{area}},</div>", "");
                filecontent = filecontent.Replace("<div>{{state}} - {{postal}}.</div>", "");
                filecontent = filecontent.Replace("<div>{{address}},</div>", "");


                filecontent = filecontent.Replace("{{postal}}", "&nbsp;");
                filecontent = filecontent.Replace("<div>{{area}},</div>", "");
                filecontent = filecontent.Replace("{{area}}", "&nbsp;");
                filecontent = filecontent.Replace("{{state}}", "&nbsp;");
                filecontent = filecontent.Replace("<div>{{address}},</div>", "");
                filecontent = filecontent.Replace("{{address}}", "&nbsp;");
                filecontent = filecontent.Replace("{{customerrefno}}", "-");
                filecontent = filecontent.Replace("{{otherref}}", "-");
                filecontent = filecontent.Replace("{{deliverynote}}", "-");
                filecontent = filecontent.Replace("{{dispatchto}}", "-");
            }


            filecontent = filecontent.Replace("{{invoiceno}}", strticketno);
            filecontent = filecontent.Replace("{{invoicedate}}", strDate);
            filecontent = filecontent.Replace("{{deliverynote}}", "-");
            filecontent = filecontent.Replace("{{buyerorderno}}", sale_quoteno);
            filecontent = filecontent.Replace("{{buyerordernodate}}", sale_quotedate);
            filecontent = filecontent.Replace("{{dispatchdocno}}", "-");
            filecontent = filecontent.Replace("{{dispatchthrough}}", deliverymode);
            filecontent = filecontent.Replace("{{dispatchdate}}", dispatchdate);

            filecontent = filecontent.Replace("{{modeofpayment}}", Sales_paymentmode);


            int initialrow = 0;

            string item_details = string.Empty;

            if (dtitem != null)
            {


                for (i = currow; i < dtitem.Rows.Count; i++)
                {

                    int checkrow = 15;

                    if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                    {
                        checkrow = 12;
                    }

                    initialrow += 1;
                    currow = i;
                    string verticalcenter = "center";
                    string str_trstyle = " style='vertical-align:" + verticalcenter + ";height:30px'";
                    if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                    {
                        str_trstyle = " style='vertical-align: top;height:60px'";
                        verticalcenter = "top";
                    }
                    if (dtitem.Rows.Count - 1 == i)
                    {
                        int totallength = 0 - initialrow;

                        if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                        {
                            totallength = 0 - initialrow;
                        }


                        if (totallength > 0)
                        {
                            verticalcenter = "top";
                            string t_length = (totallength * 50).ToString() + "px";
                            str_trstyle = " style='vertical-align:" + verticalcenter + ";height:" + t_length + @"'";

                            if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                            {
                                //str_trstyle = " style='vertical-align: " + verticalcenter + ";height:60px'";
                            }
                        }
                    }

                    string description = Convert.ToString(dtitem.Rows[i]["Name"]);
                    string HSN = Convert.ToString(dtitem.Rows[i]["HSN"]);
                    string Specification = "";
                    string SpecificationInfo = Convert.ToString(dtitem.Rows[i]["dc_serialno"]);
                    if (Specification != "")
                    {
                        description += "<br /><b>" + Specification + "</b>";
                    }
                    if (SpecificationInfo != "")
                    {
                        description += "<br /><b>" + SpecificationInfo + "</b>";
                    }

                    string sale_qty = Convert.ToString(dtitem.Rows[i]["dc_qty"]);
                    string sale_rate = Convert.ToDouble(dtitem.Rows[i]["dc_rate"]).ToString("F");
                    string sale_discount = Convert.ToDouble(dtitem.Rows[i]["dc_discount"]).ToString("F");
                    string withouttaxamt = Convert.ToDouble(dtitem.Rows[i]["withouttaxamt"]).ToString("F");
                    string taxname = Convert.ToString(dtitem.Rows[i]["taxname"]);
                    string taxrate = Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["taxrate"])).ToString("F");

                    if (Convert.ToString(dtitem.Rows[i]["taxname"]) != "")
                    {
                        description += "<br /><div style='text-align:right!important'>" + taxname + "</div>";
                        sale_rate += "<br /><div style='text-align:right!important'>&nbsp;</div>";
                        withouttaxamt += "<br /><div style='text-align:right!important'>" + taxrate + "</div>";
                    }

                    if (dc_method == "1")
                    {

                        if (isdiscount == true)
                        {
                            if (item_details == string.Empty)
                            {

                                item_details = @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align: " + verticalcenter + ";text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='2' style='vertical-align:" + verticalcenter + ";'>" + description + @"</td>                    
                                                         <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                            
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_discount + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                            }
                            else
                            {
                                item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='2' style='vertical-align:" + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                            
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + sale_discount + @"</td>
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                            }
                        }
                        else
                        {
                            if (item_details == string.Empty)
                            {

                                item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                                                                                    
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                            }
                            else
                            {
                                item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: right'>" + HSN + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_qty + @"</td>
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + sale_rate + @"</td>                                                                                    
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: right'>" + withouttaxamt + @"</td>                     
                                                        </tr>";
                            }
                        }
                    }
                    else
                    {

                        if (item_details == string.Empty)
                        {

                            item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td colspan='2' style='vertical-align:" + verticalcenter + "; text-align: center'>" + HSN + @"</td>
                                                        <td colspan='2' style='vertical-align: " + verticalcenter + "; text-align: center'>" + sale_qty + @"</td>
                                                        </tr>";
                        }
                        else
                        {
                            item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='3' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                    
                                                        <td colspan='2' style='vertical-align:" + verticalcenter + "; text-align: center'>" + HSN + @"</td>
                                                        <td colspan='2' style='vertical-align: " + verticalcenter + "; text-align: center'>" + sale_qty + @"</td>
                                                        </tr>";
                        }
                    }

                    total += Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["withouttaxamt"]));
                    totalqty += Convert.ToInt32(Convert.ToString(dtitem.Rows[i]["dc_qty"]));
                    taxamt += Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["taxrate"]));
                    itemdiscount += Convert.ToDouble(Convert.ToString(dtitem.Rows[i]["dc_discount"]));

                    if (initialrow == checkrow)
                    {
                        filecontent = filecontent.Replace("{{rowdata}}", item_details);
                        filecontent = filecontent.Replace("{{modeofpayment}}", Sales_paymentmode);





                        writer.Write(filecontent);
                        writer.Write(continuetext);

                        //writer.Write("</table>This is a Computer Generated Delivery Challan</div></div></body></html>");
                        writer.Close();
                        totalfile += 1;
                        initialrow = 0;
                        currow += 1;
                        goto ContinueFinal;
                    }

                }
            }



            filecontent = filecontent.Replace("{{rowdata}}", item_details);

            writer.Write(filecontent);
            filecontent = "";

            strpath_withdiscount = Rootpath + @"\Format\dc_halfpage_invoice_discount.html";
            strpath_withoutdiscount = Rootpath + @"\Format\dc_halfpage_invoice_withoutdiscount.html";
            strpath_withoutrate = Rootpath + @"\Format\dc_halfpage_invoice_withoutrate.html";




            isdiscount = false;

            if (dc_method == "1")
            {

                if (discountcount != 0)
                {
                    var fileStream = new FileStream(strpath_withdiscount, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        filecontent = streamReader.ReadToEnd();
                    }

                    isdiscount = true;

                }
                else
                {
                    var fileStream = new FileStream(strpath_withoutdiscount, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        filecontent = streamReader.ReadToEnd();
                    }

                }
            }
            else
            {
                var fileStream = new FileStream(strpath_withoutrate, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    filecontent = streamReader.ReadToEnd();
                }

            }



            filecontent = filecontent.Replace("{{Terms}}", Terms);
            filecontent = filecontent.Replace("{{totalrate}}", "&nbsp;");
            filecontent = filecontent.Replace("{{totalqty}}", totalqty.ToString());
            filecontent = filecontent.Replace("{{totaldiscount}}", Convert.ToDouble(itemdiscount).ToString("F"));


            string consolidatetotal = "";
            string colscnt = "7";

            consolidatetotal = (total + taxamt).ToString();
            filecontent = filecontent.Replace("{{totalamount}}", consolidatetotal);
            double totalamt = (total + taxamt) - discount;
            string consolidatedtax = "";


            string str_consolidatetaxqry = @"select                                                 
                    isnull(STUFF((SELECT ',' + (select sub.code from tbl_taxmaster sub where  sub.id=tx.dc_tax)  + '@' + CONVERT(varchar,dc_taxvalue) + '%' 
                    FROM sandd_tbl_sales_dc_tax tx where tx.dc_id=it.dc_id and ISNULL(dc_taxvalue,0)!=0 
                                                            FOR XML PATH (''), TYPE).value('.', 'varchar(max)'), 1, 1, ''),'') as taxname,
                                                            SUM(dc_taxvalue)   as dc_taxvalue                                              
                                                              FROM sandd_tbl_sales_dc_tax it where ISNULL(dc_taxvalue,0)!=0 and dc_id=@dc_id
                                                              group by dc_id";

            str_consolidatetaxqry = @"select(select sub.code from tbl_taxmaster sub where sub.id=it.dc_tax)  + '@' +  CONVERT(varchar,dc_taxvalue)  as taxname,
                                    dc_taxvalue   as dc_taxvalue   FROM sandd_tbl_sales_dc_tax it where ISNULL(dc_taxvalue,0)!=0 and it.dc_id=@dc_id";

            string con_taxname = "";
            double con_taxvalue = 0;
            ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@dc_id", Convert.ToString(dc_id).Trim());
            dtsale = new DataTable();
            dtsale = ies.Fn_ExecutiveSql_Datatable(str_consolidatetaxqry, 0, 1);

            if (dtsale != null)
            {
                if (dtsale.Rows.Count > 0)
                {

                    for (int itaxid = 0; itaxid < dtsale.Rows.Count; itaxid++)
                    {

                        con_taxname = Convert.ToString(dtsale.Rows[itaxid]["taxname"]);
                        con_taxvalue = Convert.ToDouble(dtsale.Rows[itaxid]["dc_taxvalue"]);

                        if (con_taxname != "")
                        {
                            double totalconsolidatetaxvalue = 0;
                            if (con_taxvalue != 0)
                            {
                                totalconsolidatetaxvalue = (Convert.ToDouble(total) - Convert.ToDouble(discount)) * Convert.ToDouble(Convert.ToDouble(con_taxvalue) / Convert.ToDouble(100));
                                totalamt = totalamt + totalconsolidatetaxvalue;

                            }

                            consolidatedtax += @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>" + con_taxname + @"%&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (totalconsolidatetaxvalue).ToString("F") + "</td></tr>";


                        }

                    }
                }
            }





            filecontent = filecontent.Replace("{{consolidatetax}}", consolidatedtax);

            string consolidatediscount = "";



            if (discount != 0)
            {

                consolidatediscount = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Discount&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (discount).ToString("F") + "</td></tr>";
            }

            filecontent = filecontent.Replace("{{overalldiscount}}", consolidatediscount);












            string roundoff = roundoff = (totalamt - Math.Round(totalamt, 0)).ToString("0.00");

            if (roundoff.IndexOf("-") == 0) roundoff = roundoff.Replace('-', '+');
            else roundoff = "-" + roundoff;

            string consolidateroundoff = "";
            if (roundoff != "" && roundoff != "-0.00" && roundoff != "0.00" && roundoff != "+0.00")
            {

                consolidateroundoff = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Rounded Off&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (roundoff) + "</td></tr>";
            }


            filecontent = filecontent.Replace("{{overallroundoff}}", consolidateroundoff);



            string consolidatenettotal = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Net Total&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (Math.Round(totalamt, 0)).ToString("F") + "</td></tr>";


            filecontent = filecontent.Replace("{{nettotal}}", consolidatenettotal);







            string consolidateShipmentCharge = "";


            if (Sale_shipmentcharge != 0)
            {

                consolidateShipmentCharge = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Shipment Charge&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (Sale_shipmentcharge).ToString("F") + "</td></tr>";

            }

            filecontent = filecontent.Replace("{{shipmentcharge}}", consolidateShipmentCharge);

            double grandamt = 0;


            grandamt = totalamt + Sale_shipmentcharge;

            string consolidategrandtotal = @"<tr>
                                        <td style='vertical-align: central; text-align: right;width:7%' colspan='" + colscnt + @"'>Grand Total&nbsp;</td>                                                                                                               
                                        <td style='vertical-align: central; text-align: right'>" + (Math.Round(grandamt, 0)).ToString("F") + "</td></tr>";


            filecontent = filecontent.Replace("{{grandtotal}}", consolidategrandtotal);




            string amtwords;
            string totalbillamt = (Math.Round(grandamt, 0)).ToString("F");
            IndiaCurrencyConverter con = new IndiaCurrencyConverter();
            if (IsNumeric(totalbillamt))
            {
                amtwords =
                (con.ConvertToWord(totalbillamt, System.Drawing.Color.Blue, "0") + " rupees only");
            }
            else
            {
                string amt = totalbillamt.ToString().Split('.')[0];
                string paise = Convert.ToDouble(totalbillamt).ToString("0.00").Split('.')[1];

                string totalamtinwords = "";
                string totalpaiseinwords = "";
                if (Convert.ToDouble(paise) == 0)
                {
                    totalamtinwords = con.ConvertToWord(amt, System.Drawing.Color.Blue, "1") + " rupees only";
                }
                else
                {
                    totalamtinwords = con.ConvertToWord(amt, System.Drawing.Color.Blue, "1") + " rupees and ";
                    totalpaiseinwords = con.ConvertToWord(paise, System.Drawing.Color.Blue, "1") + " paise only";
                }


                amtwords = (totalamtinwords + totalpaiseinwords);
            }

            filecontent = filecontent.Replace("{{amountinwords}}", amtwords);







            writer.Write(filecontent);
            // writer.Write("</table>This is a Computer Generated Delivery Challan</div></div></body></html>");
            writer.Close();
            //MessageBox.Show(filecontent);
            // web_report.DocumentText = filecontent;
            string strpath = Rootpath + "/Invoice";
            string domainname = "0001";
            string[] Inputfile = filelist.ToArray();
            PdfGenerator.Invoicepdf(strpath, domainname,
                Inputfile);

            //pdfViewer1.Document = new PdfDocument(@"D:\Chinna\IES\Development\Xsell_Retail\Xsell_Retail\Xsell_Retail\bin\Debug\001\0001.pdf");
            string file_pathfile = strpath + "/0001.pdf";
            //axAcroPDF1.LoadFile(file_pathfile);

        }
        catch
        {

        }

        // When a cursor is back to its default shape, it means the process ends

    }

    //-------------------------------Structures and classes----------------------------

    internal bool IsNumeric(string ValueInNumeric)
    {
        bool IsFine = true;
        foreach (char ch in ValueInNumeric)
        {
            if (!(ch >= '0' && ch <= '9'))
            {
                IsFine = false;
            }
        }
        return IsFine;
    }
}

public class Inward_DC_Pdf_invoice2
{
    public Inward_DC_Pdf_invoice2()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public Inward_DC_Pdf_invoice2(string dc_id, string strticketno, string delv_id)
    {

        string discount_print_continuetext = @"<tr>
                    <td style='vertical-align: central; text-align: left; width: 7%' colspan='7'>
                        <div style='text-align: right'><b>Continue...</b></div>
                    </td>
                </tr>";

        string sale_customername = "Unknown";
        string sale_quoteno = "";
        string sale_quotedate = "";
        string deliverymode = "";
        string dispatchdate = "";
        string buyer_tinno = "";

        string str_salesdetailqry = @"select 
                                        dc_id,dc_customername,
                                        dbo.UDF_DateFormat(dc_date,1,1) as dc_date,
                                        dbo.UDF_DateFormat(dc_date,1,0) as dispatchdate,
                                        dc_ticketno as billNo,
                                        financialyear,dc_customerid,
                                        case when ISNULL(dc_deliverymode,'')='' then 'Hand Delivery' 	                                        
	                                     else ISNULL(dc_deliverymode,'Hand Delivery') end as deliverymode,
                                        case when ISNULL(dc_buyerorderno,'Verbal')='' then 'Verbal' else  ISNULL(dc_buyerorderno,'Verbal') end  as Verbal, 
                                        case when dbo.UDF_DateFormat(ISNULL(dc_buyerorderdate,'1900-01-01'),1,0)='01-01-1900'  then dbo.UDF_DateFormat(dc_date,1,0) else dbo.UDF_DateFormat(dc_buyerorderdate,1,0) end  as dc_quotedate
                                        ,isnull(orphan_id,'0') as orphan_id,isnull(dc_method,'1') as dc_method
                                         from sandd_tbl_sales_dc_workorder where dc_id=@dc_id";

        string billmethod = "";
        string dc_method = "1";
        string Terms = "";
        string sale_custid = "";
        string orphan_custid = "";
        double discount = 0.0;
        double Sale_shipmentcharge = 0.0;
        string strDate = "";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@dc_id", Convert.ToString(dc_id).Trim());
        DataTable dtsale = new DataTable();
        dtsale = ies.Fn_ExecutiveSql_Datatable(str_salesdetailqry, 0, 1);

        if (dtsale != null)
        {
            if (dtsale.Rows.Count > 0)
            {
                strticketno = Convert.ToString(dtsale.Rows[0]["billNo"]);
                billmethod = Convert.ToString("Delivery Challan");
                dc_method = Convert.ToString(dtsale.Rows[0]["dc_method"]);
                strDate = Convert.ToString(dtsale.Rows[0]["dc_date"]);
                sale_customername = Convert.ToString(dtsale.Rows[0]["dc_customername"]);
                sale_custid = Convert.ToString(dtsale.Rows[0]["dc_customerid"]);
                orphan_custid = Convert.ToString(dtsale.Rows[0]["orphan_id"]);
                sale_quoteno = Convert.ToString(dtsale.Rows[0]["Verbal"]);
                sale_quotedate = Convert.ToString(dtsale.Rows[0]["dc_quotedate"]);
                deliverymode = Convert.ToString(dtsale.Rows[0]["deliverymode"]);
                dispatchdate = Convert.ToString(dtsale.Rows[0]["dispatchdate"]);
            }
        }



        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        string strquery = @"select top 1 * from (select distinct om.CompanyName as Company,cm.Mobile,cm.emailid,om.Area,District as[District],om.Pincode,cm.company_id,om.Address,
                                    (select st.Name from tbl_statemaster st where om.State=st.id) as State,companyno,isnull(om.TinNo,'') as TinNo,ISNULL(GSTIN,'') as GSTIN
                                    from tbl_Companycustomercontact cm inner join tbl_companycustomer om on om.company_id=cm.company_id ) T where 1=1";
        strquery += " and company_id='" + sale_custid + "'";
        DataTable dt_companycontact = ies.Fn_ExecutiveSql_Datatable(strquery, 0, 1);



        strquery = @"select delv_id as delv_id,dc_id,delivery_reference_no,invoice_no,
                            dbo.UDF_DateFormat(po.invoice_date,1,0) as  invoice_date,	                       
                            (select upper(usr_sub.Firstname)  from tbl_usermaster usr_sub where  usr_sub.Usrid=po.delivered_by) as delivered_by                           
	                        from sandd_tbl_sales_dc_workorder_delivery po where po.dc_id=@dc_id and delv_id=@delv_id";
        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@dc_id", Convert.ToString(dc_id).Trim());
        ies.ies_parameters.Add("@delv_id", Convert.ToString(delv_id).Trim());
        DataTable dt_info2 = ies.Fn_ExecutiveSql_Datatable(strquery, 0, 1);

        string DCInvoiceno = "";
        string DCInvoiceDate = "";
        if (dt_info2 != null)
        {
            if (dt_info2.Rows.Count > 0)
            {
                DCInvoiceno = Convert.ToString(dt_info2.Rows[0]["invoice_no"]);
                DCInvoiceDate = Convert.ToString(dt_info2.Rows[0]["invoice_date"]);
            }
        }



        string str_qry =
        @"select iot.dc_productname , case when ISNULL(it.dc_description,'')='NA' then '' else 
                            ISNULL(it.dc_description,'') end as ItemDescription,iot.hsn_code as HSN,
                            it.deliver_qty as qty from sandd_tbl_sales_dc_workorder_delivery_items it inner join 
                            sandd_tbl_sales_dc_workorder_items iot on iot.dc_itemid=it.dc_itemid and it.dc_id=iot.dc_id where it.dc_id=@dc_id and it.delv_id=@delv_id";

        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@dc_id", Convert.ToString(dc_id).Trim());
        ies.ies_parameters.Add("@delv_id", Convert.ToString(delv_id).Trim());
        DataTable dtitem = new DataTable();
        dtitem = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);
        string strbcData = strticketno;

        List<string> filelist = new List<string>();
        int totalfile = 0;
        string filefolder = HttpContext.Current.Server.MapPath("~/Invoice");
        if (System.IO.Directory.Exists(filefolder) == true)
        {
            DirectoryInfo dr = new DirectoryInfo(filefolder);
            foreach (var fi in dr.GetFiles())
            {
                File.Delete(fi.FullName);
            }
        }
        else
        {
            System.IO.Directory.CreateDirectory(filefolder);
        }
        string Rootpath = HttpContext.Current.Server.MapPath("~");


        string strpath_withoutrate = Rootpath + @"\Format\inward_dc2_invoice_finalwithoutrate_print2.html";

        string filecontent;
        Int32 totalqty = 0;
        int i = 0;
        int currow = 0;
        string Sales_paymentmode = "";

    ContinueFinal:

        string filepath = filefolder + @"\" + "File_" + Convert.ToString(totalfile) + ".html";
        if (File.Exists(filepath))
        {
            File.Delete(filepath);
        }
        StreamWriter writer = new StreamWriter(filepath);
        filelist.Add(filepath);

        try
        {


            bool isdiscount = false;

            string continuetext = "";


            var fileStream = new FileStream(strpath_withoutrate, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                filecontent = streamReader.ReadToEnd();
            }
            continuetext = discount_print_continuetext;






            if (dt_companycontact != null)
            {
                if (dt_companycontact.Rows.Count > 0)
                {

                    buyer_tinno = Convert.ToString(dt_companycontact.Rows[0]["TinNo"]);
                    string buyer_GSTIN = Convert.ToString(dt_companycontact.Rows[0]["GSTIN"]);
                    filecontent = filecontent.Replace("{{companyname}}", Convert.ToString(dt_companycontact.Rows[0]["Company"]));


                    filecontent = filecontent.Replace("{{area}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));

                    filecontent = filecontent.Replace("{{state}}", Convert.ToString(dt_companycontact.Rows[0]["District"]) + "," + Convert.ToString(dt_companycontact.Rows[0]["state"]));
                    if (buyer_tinno != "" && buyer_GSTIN != "")
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".<br>" + "Buyer's Sales Tax No." + buyer_tinno + "<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                    }
                    else if (buyer_tinno == "" && buyer_GSTIN != "")
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                    }
                    else
                    {
                        filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".");
                    }

                    filecontent = filecontent.Replace("{{address}}", Convert.ToString(dt_companycontact.Rows[0]["address"]));

                    filecontent = filecontent.Replace("{{customerrefno}}", Convert.ToString(dt_companycontact.Rows[0]["companyno"]));

                    filecontent = filecontent.Replace("{{otherref}}", "-");
                    filecontent = filecontent.Replace("{{deliverynote}}", "-");
                    filecontent = filecontent.Replace("{{dispatchto}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));



                }
                else
                {

                    ies = new IES_Generic_Function();
                    ies.ies_parameters.Clear();
                    strquery = @"select isnull(om.CompanyName,'') as Company,
                                    ISNULL(om.Mobileno,'') as Mobile,isnull(om.emailid,'') as emailid,
                                    isnull(om.Area,'') as Area,
                                    isnull(District,'') as[District],
                                    isnull(om.Pincode,'') as Pincode,'' as Address,
                                    '' as State,'' as companyno,
                                    isnull(om.TinNo,'') as TinNo,
                                    ISNULL(GSTIN,'') as GSTIN,
                                    '0' as company_id
                                    from tbl_orphancustomer om where 1=1 ";
                    strquery += " and company_id='" + orphan_custid + "'";
                    dt_companycontact = ies.Fn_ExecutiveSql_Datatable(strquery, 0, 1);


                    if (dt_companycontact != null)
                    {
                        if (dt_companycontact.Rows.Count > 0)
                        {

                            buyer_tinno = Convert.ToString(dt_companycontact.Rows[0]["TinNo"]);
                            string buyer_GSTIN = Convert.ToString(dt_companycontact.Rows[0]["GSTIN"]);
                            filecontent = filecontent.Replace("{{companyname}}", Convert.ToString(dt_companycontact.Rows[0]["Company"]));
                            if (Convert.ToString(dt_companycontact.Rows[0]["area"]) == "")
                            {
                                filecontent = filecontent.Replace("{{area}},", "");
                            }
                            else
                            {
                                filecontent = filecontent.Replace("{{area}},", Convert.ToString(dt_companycontact.Rows[0]["area"]) + ",");
                            }
                            if (Convert.ToString(dt_companycontact.Rows[0]["District"]) == "")
                            {
                                filecontent = filecontent.Replace("{{state}} - ", "");
                            }
                            else
                            {
                                filecontent = filecontent.Replace("{{state}} - ", Convert.ToString(dt_companycontact.Rows[0]["District"]) + ",");
                            }

                            if (buyer_tinno != "" && buyer_GSTIN != "")
                            {
                                filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + "<br>" + "Buyer's Sales Tax No." + buyer_tinno + "<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                            }
                            else if (buyer_tinno == "" && buyer_GSTIN != "")
                            {
                                filecontent = filecontent.Replace("{{postal}}", Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + "<br>" + "Buyer's GSTIN." + buyer_GSTIN);
                            }
                            else
                            {

                                if (Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) == "")
                                {
                                    filecontent = filecontent.Replace("{{postal}}", "");
                                }
                                else
                                {
                                    filecontent = filecontent.Replace("{{postal}}", "<br>" + Convert.ToString(dt_companycontact.Rows[0]["Pincode"]) + ".");
                                }

                            }
                            filecontent = filecontent.Replace("{{dispatchto}}", Convert.ToString(dt_companycontact.Rows[0]["area"]));
                        }
                    }

                    filecontent = filecontent.Replace("{{companyname}}", sale_customername);
                    filecontent = filecontent.Replace("{{postal}}", "&nbsp;");
                    filecontent = filecontent.Replace("{{area}}", "&nbsp;");
                    filecontent = filecontent.Replace("{{area}},", "&nbsp;");
                    filecontent = filecontent.Replace("{{state}}", "&nbsp;");
                    filecontent = filecontent.Replace("<div>{{address}},</div>", "");
                    filecontent = filecontent.Replace("{{address}}", "&nbsp;");
                    filecontent = filecontent.Replace("{{address}},", "&nbsp;");
                    filecontent = filecontent.Replace("{{customerrefno}}", "-");
                    filecontent = filecontent.Replace("{{otherref}}", "-");
                    filecontent = filecontent.Replace("{{deliverynote}}", "-");
                    filecontent = filecontent.Replace("{{dispatchto}}", "-");
                }
            }
            else
            {
                filecontent = filecontent.Replace("{{companyname}}", sale_customername);

                filecontent = filecontent.Replace("<div>{{area}},</div>", "");
                filecontent = filecontent.Replace("<div>{{state}} - {{postal}}.</div>", "");
                filecontent = filecontent.Replace("<div>{{address}},</div>", "");


                filecontent = filecontent.Replace("{{postal}}", "&nbsp;");
                filecontent = filecontent.Replace("<div>{{area}},</div>", "");
                filecontent = filecontent.Replace("{{area}}", "&nbsp;");
                filecontent = filecontent.Replace("{{state}}", "&nbsp;");
                filecontent = filecontent.Replace("<div>{{address}},</div>", "");
                filecontent = filecontent.Replace("{{address}}", "&nbsp;");
                filecontent = filecontent.Replace("{{customerrefno}}", "-");
                filecontent = filecontent.Replace("{{otherref}}", "-");
                filecontent = filecontent.Replace("{{deliverynote}}", "-");
                filecontent = filecontent.Replace("{{dispatchto}}", "-");
            }





            filecontent = filecontent.Replace("{{invoiceno}}", strticketno);
            filecontent = filecontent.Replace("{{invoicedate}}", strDate);
            filecontent = filecontent.Replace("{{deliverynote}}", "-");
            filecontent = filecontent.Replace("{{buyerorderno}}", sale_quoteno);
            filecontent = filecontent.Replace("{{buyerordernodate}}", sale_quotedate);
            filecontent = filecontent.Replace("{{dispatchdocno}}", "-");
            filecontent = filecontent.Replace("{{dispatchthrough}}", deliverymode);
            filecontent = filecontent.Replace("{{dispatchdate}}", dispatchdate);

            filecontent = filecontent.Replace("{{modeofpayment}}", Sales_paymentmode);


            int initialrow = 0;

            string item_details = string.Empty;

            if (dtitem != null)
            {


                for (i = currow; i < dtitem.Rows.Count; i++)
                {

                    int checkrow = 15;



                    initialrow += 1;
                    currow = i;
                    string verticalcenter = "center";
                    string str_trstyle = " style='vertical-align:" + verticalcenter + ";height:30px'";

                    if (dtitem.Rows.Count - 1 == i)
                    {
                        int totallength = 15 - initialrow;
                        totallength = 0;
                        if (totallength > 0)
                        {
                            verticalcenter = "top";
                            string t_length = (totallength * 45).ToString() + "px";
                            str_trstyle = " style='vertical-align:" + verticalcenter + ";height:" + t_length + @"'";
                        }
                    }

                    string description = Convert.ToString(dtitem.Rows[i]["dc_productname"]);


                    string HSN = Convert.ToString(dtitem.Rows[i]["HSN"]);
                    string Specification = Convert.ToString(dtitem.Rows[i]["ItemDescription"]); ;
                    string SpecificationInfo = "";
                    if (Specification != "")
                    {
                        description += "<br /><b>" + Specification + "</b>";
                    }
                    if (SpecificationInfo != "")
                    {
                        description += "<br /><b>" + SpecificationInfo + "</b>";
                    }

                    string sale_qty = Convert.ToString(dtitem.Rows[i]["qty"]);



                    if (item_details == string.Empty)
                    {

                        item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align:" + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='5' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                                                                            
                                                        <td colspan='2' style='vertical-align: " + verticalcenter + "; text-align: center'>" + sale_qty + @"</td>
                                                        </tr>";
                    }
                    else
                    {
                        item_details += @"<tr" + str_trstyle + @">
                                                        <td style='vertical-align: " + verticalcenter + "; text-align: center;width:7%'>" + (i + 1).ToString() + @".</td>
                                                        <td  colspan='5' style='vertical-align: " + verticalcenter + ";'>" + description + @"</td>                                                                            
                                                        <td colspan='2' style='vertical-align: " + verticalcenter + "; text-align: center'>" + sale_qty + @"</td>
                                                        </tr>";
                    }



                    totalqty += Convert.ToInt32(Convert.ToString(dtitem.Rows[i]["qty"]));


                    if (initialrow == checkrow)
                    {
                        filecontent = filecontent.Replace("{{rowdata}}", item_details);
                        filecontent = filecontent.Replace("{{modeofpayment}}", Sales_paymentmode);

                        writer.Write(filecontent);
                        writer.Write(continuetext);

                        //writer.Write("</table>This is a Computer Generated Delivery Challan</div></div></body></html>");
                        writer.Close();
                        totalfile += 1;
                        initialrow = 0;
                        currow += 1;
                        goto ContinueFinal;
                    }

                }
            }



            filecontent = filecontent.Replace("{{rowdata}}", item_details);

            writer.Write(filecontent);
            filecontent = "";


            strpath_withoutrate = Rootpath + @"\Format\inward_dc2_invoice_withoutrate_print2.html";




            isdiscount = false;


            fileStream = new FileStream(strpath_withoutrate, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                filecontent = streamReader.ReadToEnd();
            }





            filecontent = filecontent.Replace("{{Terms}}", Terms);
            filecontent = filecontent.Replace("{{totalrate}}", "&nbsp;");
            filecontent = filecontent.Replace("{{totalqty}}", totalqty.ToString());

            filecontent = filecontent.Replace("{{DCinvoiceno}}", DCInvoiceno);
            filecontent = filecontent.Replace("{{DCinvoicedate}}", DCInvoiceDate);


            string consolidatetotal = "";
            filecontent = filecontent.Replace("{{totalamount}}", consolidatetotal);

            writer.Write(filecontent);
            // writer.Write("</table>This is a Computer Generated Delivery Challan</div></div></body></html>");
            writer.Close();
            //MessageBox.Show(filecontent);
            // web_report.DocumentText = filecontent;
            string strpath = Rootpath + "/Invoice";
            string domainname = "0001";
            string[] Inputfile = filelist.ToArray();
            PdfGenerator.Invoicepdf(strpath, domainname,
                Inputfile);

            //pdfViewer1.Document = new PdfDocument(@"D:\Chinna\IES\Development\Xsell_Retail\Xsell_Retail\Xsell_Retail\bin\Debug\001\0001.pdf");
            string file_pathfile = strpath + "/0001.pdf";
            //axAcroPDF1.LoadFile(file_pathfile);

        }
        catch
        {

        }

        // When a cursor is back to its default shape, it means the process ends

    }

    //-------------------------------Structures and classes----------------------------

    internal bool IsNumeric(string ValueInNumeric)
    {
        bool IsFine = true;
        foreach (char ch in ValueInNumeric)
        {
            if (!(ch >= '0' && ch <= '9'))
            {
                IsFine = false;
            }
        }
        return IsFine;
    }
}



