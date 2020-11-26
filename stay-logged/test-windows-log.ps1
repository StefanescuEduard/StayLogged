$evt = new-object System.Diagnostics.EventLog("Application")
$evt.Source = "MyEvent"
$infoevent = [System.Diagnostics.EventLogEntryType]::Error
$evt.WriteEntry("Error happened",$infoevent,70)