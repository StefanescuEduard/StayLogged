$evt = new-object System.Diagnostics.EventLog("Application")
$evt.Source = "MyEvent"
$infoevent = [System.Diagnostics.EventLogEntryType]::Information
$evt.WriteEntry("My Test Event",$infoevent,70)