﻿$(document).ready () ->
  $('#migrateElectionFromDir').submit (e) ->
    $.ajax
      url:  '/Migration/DirToElection',
      type: 'POST',
      data: $(this).serialize(),
      success: (data) ->
        alert data
    e.preventDefault()
  