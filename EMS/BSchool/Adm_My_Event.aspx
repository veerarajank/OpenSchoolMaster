<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_MyAccount.master" AutoEventWireup="true" CodeFile="Adm_My_Event.aspx.cs" Inherits="Adm_My_Event" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
     
    <script type="text/javascript">
        $(document).ready(function () {
            $.ajax({
                type: "POST",
                contentType: "application/json",
                data: "{}",
                url: "Adm_My_Event.aspx/GetEvents",
                dataType: "json",
                success: function (data) {
                    $('div[id*=fullcal]').fullCalendar({
                        header: {
                            left: 'prev,next today',
                            center: 'title',
                            right: 'month,agendaWeek,agendaDay'
                        },
                        editable: true,
                        events: $.map(data.d, function (item, i) {
                            var event = new Object();
                            event.id = item.Event_id;
                            event.start = new Date(item.Event_start);
                            event.end = new Date(item.Event_end);
                            event.title = item.Event_title;
                            event.type = item.Event_type;
                            event.color = item.Event_Color;
                            return event;
                        }), eventRender: function (event, eventElement) {
                            if (event.color) {
                                if (eventElement.find('span.fc-event-time').length) {
                                    eventElement.find('span.fc-event-time').before($(GetColor(event.color)));
                                } else {
                                    eventElement.find('span.fc-event-title').before($(GetColor(event.color)));
                                }
                            }
                        },
                        loading: function (bool) {
                            if (bool) $('#loading').show();
                            else $('#loading').hide();
                        }
                    });
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                }
            });
            $('#loading').hide();
            $('div[id*=fullcal]').show();
        });
        function GetColor(type) {
            if (type == 0) {
                return "<br/><img src = 'EventCal/Styles/Images/attendance.png' style='width:24px;height:24px'/><br/>"
            }
            else if (type == 1) {
                return "<br/><img src = 'EventCal/Styles/Images/not_available.png' style='width:24px;height:24px'/><br/>"
            }
            else
                return "<br/><img src = 'EventCal/Styles/Images/not_available.png' style='width:24px;height:24px'/><br/>"
        }

    </script>

    <div class="cont_right formWrapper">
                        <h1>Calendar</h1>

    <div id="loading">
        <img src="EventCal/Styles/images/loading_wh.gif" />
    </div>
    <div id="fullcal" class="fc ui-widget">
    </div>
    

        </div>
</asp:Content>

