<?php

	$user = $_POST["Input_user"];
	$pass = $_POST["Input_pass"];
	$pass_new = $_POST["Input_update_pass"];
	$memo = $_POST["Input_Info"];

	//error_reporting(E_ALL);
    //ini_set("display_errors", 1);

	//$user = '�ڼ���';
	$user = iconv('utf-8', 'utf-8', $user);
	//euc-kr > utf-8
	//$user = iconv('utf-8', 'utf-8', $user);
	//$memo �� �ѱ�� ���� �� �����Ƿ�, ���ڵ� ������ �ذ�����
	$memo = iconv('utf-8', 'utf-8', $memo);
	
	$conn=mysqli_connect("localhost","root","1234");
		
	//if(mysqli_connect_errno($conn))
	//{
	//	echo "Fail to connect to MYSQL: " . msqli_connect_error();
	//}
	mysqli_set_charset($conn,"utf8");
	
	mysqli_select_db($conn, "study");

	//���� �ش� ���̵� �����ϴ��� �Ǵ��ϱ� ���ؼ�
	//SELECT ������ ��������
	$query = "SELECT * FROM user WHERE userid='$user'";

	//echo $query;

	$res = mysqli_query($conn, $query);	
	//������� ������ numrows�� ����
	//������� �������� ������ 0�� ��ȯ
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