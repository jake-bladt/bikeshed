$(document).ready -> $('#migrateEFD').click (e) ->
  $.ajax
    url:  '/Migration/DirToElection'
    , type: 'POST'
    , data: 
      dirPath: "foo"
      , electionName: "eName"
      , eventDate: "eDate"
    , success: (data) -> alert data
  
  e.preventDefault()    
  