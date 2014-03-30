$(document).ready -> $('#migrateEFD').click (e) ->
  $.ajax
    url:  '/Migration/DirToElection'
    , type: 'POST'
    , data: $('#migrateElectionsFromDir').serialize()
    , success: (data) -> alert data
  
  e.preventDefault()    
  