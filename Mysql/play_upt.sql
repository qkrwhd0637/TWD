CREATE DEFINER=`root`@`localhost` PROCEDURE `play_upt`(IN in_id VARCHAR(20), IN in_round INT)
BEGIN
	DECLARE cnt INT;
	SET cnt:= 0;
    
	SELECT COUNT(*) INTO cnt
    FROM play_tb a,
		 user_tb b
    WHERE a.id = b.id
	AND a.id = in_id;
    
    #존재하는 아이디가 없다면 추가한다.
    IF cnt = 0
		THEN 
			INSERT INTO play_tb
			VALUES 
			(
				in_id,
				in_round,
				in_round
			 );
	 #존재하는 아이디가 있다면 업데이트
	ELSEIF cnt > 0 
		THEN
			UPDATE play_tb a
            SET a.top_round = CASE
							   WHEN a.top_round < in_round
							   THEN in_round
							   ELSE a.top_round END,
			    a.last_round = in_round
			WHERE a.id = in_id;
	END IF;
END