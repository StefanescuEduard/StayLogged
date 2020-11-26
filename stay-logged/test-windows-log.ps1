$evt = new-object System.Diagnostics.EventLog("Application")
$evt.Source = "MyEvent2"
$infoevent = [System.Diagnostics.EventLogEntryType]::Information
$evt.WriteEntry("Test",$infoevent,70)