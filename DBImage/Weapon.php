<?php
	// 무기 UID 값을 받아와서 그 무기와 관련된 모든 데이터를 반환해주는게 목표
	// 현재 사용하는 DB 이름은 ProjectDB
	// 테이블의 이름은 Weapon // Class // 
	// column의 이름은 
	// Weapon
	// Weapon_UID // Weapon_Name // Weapon_Damage // Weapon_AttackSpeed // Weapon_AttackDistance // Weapon_ReloadBullet // Weapon_CarryBullet // Weapon_ReloadTime // Weapon_AttackRange

	$weapon_UID = $_POST["Input_weaponUID"];

	//error_reporting(E_ALL);
    //ini_set("display_errors", 1);

	//$user = '박성훈';
	$user = iconv('utf-8', 'utf-8', $user);
	//euc-kr > utf-8
	//$user = iconv('utf-8', 'utf-8', $user);
	//$memo 도 한국어가 들어올 수 있으므로, 인코딩 문제를 해결해줌
	$memo = iconv('utf-8', 'utf-8', $memo);
	
	$conn=mysqli_connect("localhost","root","1234");
		
	//if(mysqli_connect_errno($conn))
	//{
	//	echo "Fail to connect to MYSQL: " . msqli_connect_error();
	//}
	mysqli_set_charset($conn,"utf8");
	mysqli_select_db($conn, "study");

	//먼저 해당 아이디가 존재하는지 판단하기 위해서
	//SELECT 쿼리를 수행해줌
	$query = "SELECT * FROM user WHERE userid='$user'";

	//echo $query;

	$res = mysqli_query($conn, $query);	
	//결과값의 갯수를 numrows에 저장
	//결과값이 존재하지 않으면 0을 반환
	$numrows = mysqli_num_rows($res);    

	//echo $numrows;

	if($numrows > 0)
	{
		//UPDATE `user` SET `id`=[value-1],`password`=[value-2],`note`=[value-3] WHERE 1
		$query = "UPDATE `user` SET `userId`='$user',`userpw`='$pass_new',`usernote`='$memo' WHERE `userid` = '$user'";
		echo $query;

		$res = mysqli_query($conn, $query);	

		if($res)
			die("Update Success. \n");
		else
			die("Update error. \n");
	}
	else
		die("Not Exists. \n");
	
	mysqli_close($conn);	
?>