var tempEvent;
(function($, undefined) { 
    $.fn.eventCal = function(params)
    {
		var url = 'list';
		var parts = window.location.search.substr(1).split("&");
        var $_GET = {};
        for (var i = 0; i < parts.length; i++) {
        var temp = parts[i].split("=");
			  $_GET[decodeURIComponent(temp[0])] = decodeURIComponent(temp[1]);
		}
		if($_GET.type)
		{
			
			url = 'list&type='+$_GET.type;
		}
		
        var defaultCalendarOptions = {
			
            events: params.baseUrl+url,
            selectable: true,
            droppable: true,
            dropAccept:'.list-event',
            drop: function(date, allDay) {
                var originalEventObject = $(this).data('eventObject');
                var copiedEventObject = $.extend({}, originalEventObject);
                copiedEventObject.start = date;
                var dateEnd = new Date(date);
                dateEnd.setMinutes(dateEnd.getMinutes()+params.calendar.slotMinutes*2);
                copiedEventObject.end = dateEnd;
                copiedEventObject.allDay = allDay;
                copiedEventObject.editable = true;
                $.post(params.baseUrl+'update',
                {
                    eventId :0,
                    start   :copiedEventObject.start.getTime()/1000,
                    title   : copiedEventObject.title,
					desc   : copiedEventObject.desc,
					type   : copiedEventObject.type,
                    end     : copiedEventObject.end.getTime()/1000,
                    allDay  :copiedEventObject.allDay,
                    editable: true
                },
                function(returnId)
                {
                    copiedEventObject.id = returnId;
                    $('#EventCal').fullCalendar('renderEvent', copiedEventObject);
                    
                });
                if ($('#drop-remove').is(':checked'))
                {
                    $(this).remove();
                    $.post(params.baseUrl+'removeHelper',
                    {
                        title : copiedEventObject.title
                    });
                }
            },
            loading: function(bool) {
                if (bool) $('#loading').show();
                else $('#loading').hide();
            },
            eventDrop: function(event, dayDelta, minuteDelta, allDay)
            {
                if(!event.editable) return false;
                $.post(params.baseUrl+'move',
                {
					YII_CSRF_TOKEN: $("input[name='YII_CSRF_TOKEN']").val(),
                    eventId:event.id,
                    delta:(dayDelta*86400+60*minuteDelta),
                    allDay:allDay
                } );
                return false;
            },
            eventResize: function(event, dayDelta, minuteDelta)
            {
                if(!event.editable) return false;
                $.post(params.baseUrl+'resize',
                {
                    eventId:event.id,
                    delta:(dayDelta*86400+60*minuteDelta)
                } );
                return false;
            },
            eventClick: function(event)
            { 
				//alert(JSON.stringify(event));
                if(!event.editable) return false;
                tempEvent = event;
                $("#EventCal_id").val(event.id);
                var organizer = event.organizer;
                var organizer_data = organizer.replace(/&amp;/g, "&");
				$("#EventCal_organizer").val(organizer_data);
                var title = event.title;
                var title_data = title.replace(/&amp;/g, "&");
                $("#EventCal_title").val(title_data);
				$("#EventCal_type").val(event.type);
				$("#EventCal_placeholder").val(event.placeholder);
                var desc = event.desc;
                var desc_data = desc.replace(/&amp;/g, "&");
				$("#EventCal_desc").val(desc_data);
                setupStartAndEndTimeFields(event.start, event.end);
                $("input[name=EventCal_allDay]").attr('checked', event.allDay);
                $("input[name=EventCal_editable]").attr('checked', event.editable);
				$("#EventCal_delete").show();
                $('#dlg_EventCal').dialog("open");
                return false;
            },
            dayClick: function(date, allDay)
            { 
				//alert(JSON.stringify(date));
                tempEvent = new Object();
                $("#EventCal_id").val(0);
                $("#EventCal_title").val('');
				$("#EventCal_type").val('');
				$("#EventCal_organizer").val('');
				$("#EventCal_placeholder").val("");
				$("#EventCal_desc").val("");
                $("input[name=EventCal_allDay]").attr('checked', allDay);
                $("input[name=EventCal_editable]").attr('checked', true);
				$("#EventCal_delete").hide();
                var dateEnd = new Date(date);
                dateEnd.setMinutes(dateEnd.getMinutes()+params.calendar.slotMinutes*2);
                setupStartAndEndTimeFields(date, dateEnd);
                $('#dlg_EventCal').dialog("open");
                return false;
            }
        }

        /*
             * Sets up the start and end time fields in the calendar event
             * form for editing based on the calendar event being edited
             */
        setupStartAndEndTimeFields = function(dateStart, dateEnd)
        {
            var minHours, minMin, maxHours, maxMin, tempDate;
            var minArr = params.calendar.minTime.split(":");
            var maxArr = params.calendar.maxTime.split(":");
            minHours = parseInt(minArr[0]);
            minMin = parseInt(minArr[1]);
            maxHours = parseInt(maxArr[0]);
            maxMin = parseInt(maxArr[1]);
            tempDate = new Date(dateStart);
            $('#EventCal_start').empty();
            $('#EventCal_end').empty();
		      
              
            var timeSlot = minMin;
            for (var i = minHours; i <= maxHours; i++)
            {
                while(timeSlot < 60) {
                    var i_str = ( i<10 )? '0'+i : i;
                    tempDate.setHours(i);
                    tempDate.setMinutes(timeSlot);
                    var timeSlot_str = ( timeSlot < 10 )? '0'+timeSlot : timeSlot;
                    var t = i_str + ":" + timeSlot_str;
                    var selStart = (tempDate.getTime() == dateStart.getTime())? ' selected' : '';
                    var selEnd = (tempDate.getTime() == dateEnd.getTime())? ' selected' : '';
                    $('#EventCal_start').append('<option'+selStart+' value="'+tempDate.getTime()/1000+'">' + t + '</option>');
                    $('#EventCal_end').append('<option'+selEnd+' value="'+tempDate.getTime()/1000+'">' + t + '</option>');
                    timeSlot += params.calendar.slotMinutes;
                    if(i == maxHours && timeSlot>maxMin) timeSlot = 60;
                }
                timeSlot = 0;
            }

            if(eventTime(dateStart)=="0:0:0"){
                $('select#EventCal_end option:last').attr('selected','selected');
            }
        }

        eventTime = function(datetime){
            var hours = datetime.getHours(); //returns 0-23
            var minutes = datetime.getMinutes(); //returns 0-59
            var seconds = datetime.getSeconds(); //returns 0-59
           
            return hours+":"+minutes+":"+seconds;
        }


		eventDialogDelete = function()
        {
			var data =
            {
				
				eventId: $("#EventCal_id").val(),
                title: $("#EventCal_title").val(),
				organizer: $("#EventCal_organizer").val(),
				desc: $("#EventCal_desc").val(),
				//type: $("#EventCal_type").val(),
                start: $("#EventCal_start").val(),
                end: $("#EventCal_end").val(),
                allDay: $("#EventCal_allDay").is(':checked'),
                editable: $("#EventCal_editable").is(':checked'),
				placeholder: $("#EventCal_placeholder").val(),
				YII_CSRF_TOKEN:$("input[name=\'YII_CSRF_TOKEN\']").val(),
            }
			$("#EventCal_desc").val("");
			$("#EventCal_type").val("");
			$("#EventCal_placeholder").val("");
			$("#EventCal_delete").hide();
            $.post(params.baseUrl+"delete",
                data,
                function(returnId)
                {
                    switch(returnId)
                    {
                        case "0":
                            $("#EventCal").fullCalendar( 'removeEvents', tempEvent.id);
                            break;
                        
                    }
					window.location.reload();
                }
                );
            $('#dlg_EventCal').dialog("close");
        }

        eventDialogOK = function()
        {
			var data =
            {
				
				eventId: $("#EventCal_id").val(),
                title: $("#EventCal_title").val(),
				organizer: $("#EventCal_organizer").val(),
				desc: $("#EventCal_desc").val(),
				type: $("#EventCal_type").val(),
                start: $("#EventCal_start").val(),
                end: $("#EventCal_end").val(),
                allDay: $("#EventCal_allDay").is(':checked'),
                editable: $("#EventCal_editable").is(':checked'),
				placeholder: $("#EventCal_placeholder").val(),
				YII_CSRF_TOKEN:$("input[name=\'YII_CSRF_TOKEN\']").val(),
            }
			$("#EventCal_delete").hide();
            $.post(params.baseUrl+"update",
                data,
                function(returnId)
                {
					$(".errorMessage").remove();
					if(isNaN(returnId)){
						$("#EventCal_ok").attr("disabled",false);
						var errors	= JSON.parse(returnId);
						$.each(errors, function(key, message) {							
							var error	= $("<div class='errorMessage' />");
							error.text(message[0]);
							error.insertAfter($("#EventCal_" + key));
						});
					}
					else{
						switch(returnId)
						{
							case "0":
								tempEvent.id = returnId;
								$("#EventCal").fullCalendar( 'removeEvents', tempEvent.id);
								break;
							case data.eventId:
								tempEvent.id = returnId;
								tempEvent.title = data.title;
								tempEvent.organizer = data.organizer;
								tempEvent.desc = data.desc;
								//tempEvent.type = data.type;
								tempEvent.start = data.start;
								tempEvent.end = data.end;
								tempEvent.allDay = data.allDay;
								tempEvent.editable = data.editable;
								tempEvent.placeholder = data.placeholder;
								$("#EventCal").fullCalendar( 'updateEvent', tempEvent);
								break;
							default:
								tempEvent.id = returnId;
								tempEvent.title = data.title;
								tempEvent.organizer = data.organizer;
								tempEvent.desc = data.desc;
								//tempEvent.type = data.type;
								tempEvent.start = data.start;
								tempEvent.end = data.end;
								tempEvent.allDay = data.allDay;
								tempEvent.editable = data.editable;
								tempEvent.placeholder = data.placeholder;
								$("#EventCal").fullCalendar( 'renderEvent', tempEvent);
								break;
						}
						$("#EventCal_desc").val("");
						$("#EventCal_type").val("");
						$("#EventCal_placeholder").val("");
						$('#dlg_EventCal').dialog("close");
						window.location.reload();
					}
                }
            );            
        }

        createNewEvent = function()
        {
            var newTitle = prompt(params.newEventPromt, "");
            if(newTitle) {
                $.post(params.baseUrl+"createHelper",
                {
                    title : newTitle
                });
                $('#event-scrooler').append("<div class='list-event'>"+newTitle+"</div>");
                setEventsDraggable();
            }
        }

        userpreferenceOK = function()
        {
            var data = $('#dlg_Userpreference > form').serialize();
            $.post(params.baseUrl+"userpreference", data);
            $('#dlg_Userpreference').dialog("close");
        }

        params.calendar = $.extend(true, defaultCalendarOptions, params.calendar);
        $("#EventCal").fullCalendar(defaultCalendarOptions);
    }
})(jQuery);

setEventsDraggable = function()
{
    $("#event-scrooler div.list-event").each(function() {
        var eventObject = {
            title: $.trim($(this).text())
        };
        $(this).data("eventObject", eventObject);
        $(this).draggable({
            zIndex: 999,
            helper: function (e,ui) {
                return $(this).clone().appendTo('body').css('zIndex',999).show();
            } ,
            revert: true,
            revertDuration: 300
        });
    });
}

$(document).ready(function() { setEventsDraggable(); });



