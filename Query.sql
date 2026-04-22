UPDATE Hearings
SET HearingDate = GETDATE()
WHERE HearingDate = '0001-01-01'