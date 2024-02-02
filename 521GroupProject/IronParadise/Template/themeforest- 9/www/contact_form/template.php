<?php
ob_start();
?>
<html>
	<head>
	</head>
	<body>
		<div><b>First and last name</b>: <?php echo $values["name"]; ?></div>
		<div><b>E-mail</b>: <?php echo $values["email"]; ?></div>
		<?php 
		if($_POST["website"]!="" && $_POST["website"]!=_def_website)
		{
		?>
		<div><b>Website</b>: <?php echo $values["website"]; ?></div>
		<?php
		}
		?>
		<div><b>Message</b>: <?php echo nl2br($values["message"]); ?></div>
	</body>
</html>
<?php
$content = ob_get_contents();
ob_end_clean();
return($content);
?>	