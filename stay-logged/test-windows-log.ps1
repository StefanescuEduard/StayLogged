$evt = new-object System.Diagnostics.EventLog("Application")
$evt.Source = "MyEvent2"
$infoevent = [System.Diagnostics.EventLogEntryType]::Error
$evt.WriteEntry("Error happened",$infoevent,70)