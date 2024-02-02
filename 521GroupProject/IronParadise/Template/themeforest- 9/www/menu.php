<ul class="sf-menu header_right">
	<li<?php echo (!isset($_GET["page"]) || $_GET["page"]=="" || $_GET["page"]=="home" ? " class='selected'" : ""); ?>>
		<a href="?page=home" title="HOME">
			HOME
		</a>
	</li>
	<li<?php echo (isset($_GET["page"]) && ($_GET["page"]=="blog" || $_GET["page"]=="post") ? " class='selected'" : ""); ?>>
		<a href="?page=blog" title="NEWS">
			NEWS
		</a>
		<ul>
			<li>
				<a href="?page=blog" title="BLOG">
					BLOG
				</a>
			</li>
			<li>
				<a href="?page=post" title="SINGLE POST">
					SINGLE POST
				</a>
			</li>
		</ul>
	</li>
	<li<?php echo (isset($_GET["page"]) && $_GET["page"]=="classes" ? " class='selected'" : ""); ?>>
		<a href="?page=classes" title="CLASSES">
			CLASSES
		</a>
		<ul>
			<li>
				<a href="?page=classes#gym-fitness" title="GYM FITNESS">
					GYM FITNESS
				</a>
			</li>
			<li>
				<a href="?page=classes#indoor-cycling" title="INDOOR CYCLING">
					INDOOR CYCLING
				</a>
			</li>
			<li>
				<a href="?page=classes#yoga-pilates" title="YOGA PILATES">
					YOGA PILATES
				</a>
			</li>
			<li>
				<a href="?page=classes#cardio-fitness" title="CARDIO FITNESS">
					CARDIO FITNESS
				</a>
			</li>
			<li>
				<a href="?page=classes#boxing" title="BOXING">
					BOXING
				</a>
			</li>
		</ul>
	</li>
	<li<?php echo (isset($_GET["page"]) && $_GET["page"]=="timetable" ? " class='selected'" : ""); ?>>
		<a href="?page=timetable" title="TIMETABLE">
			TIMETABLE
		</a>
		<ul>
			<li>
				<a href="?page=timetable#gym-fitness" title="GYM FITNESS">
					GYM FITNESS
				</a>
			</li>
			<li>
				<a href="?page=timetable#indoor-cycling" title="INDOOR CYCLING">
					INDOOR CYCLING
				</a>
			</li>
			<li>
				<a href="?page=timetable#yoga-pilates" title="YOGA PILATES">
					YOGA PILATES
				</a>
			</li>
			<li>
				<a href="?page=timetable#cardio-fitness" title="CARDIO FITNESS">
					CARDIO FITNESS
				</a>
			</li>
			<li>
				<a href="?page=timetable#boxing" title="BOXING">
					BOXING
				</a>
			</li>
		</ul>
	</li>
	<li<?php echo (isset($_GET["page"]) && $_GET["page"]=="gallery" ? " class='selected'" : ""); ?>>
		<a href="?page=gallery" title="GALLERY">
			GALLERY
		</a>
		<ul style="margin-left: -47px; background-position: 70px 30px;">
			<li>
				<a href="?page=gallery#filter=.gym-fitness" title="GYM FITNESS">
					GYM FITNESS
				</a>
			</li>
			<li>
				<a href="?page=gallery#filter=.indoor-cycling" title="INDOOR CYCLING">
					INDOOR CYCLING
				</a>
			</li>
			<li>
				<a href="?page=gallery#filter=.yoga-pilates" title="YOGA PILATES">
					YOGA PILATES
				</a>
			</li>
			<li>
				<a href="?page=gallery#filter=.cardio-fitness" title="CARDIO FITNESS">
					CARDIO FITNESS
				</a>
			</li>
			<li>
				<a href="?page=gallery#filter=.boxing" title="BOXING">
					BOXING
				</a>
			</li>
		</ul>
	</li>
	<li<?php echo (isset($_GET["page"]) && $_GET["page"]=="contact" ? " class='selected'" : ""); ?>>
		<a href="?page=contact" title="CONTACT">
			CONTACT
		</a>
	</li>
</ul>
<div class="mobile_menu">
	<select>
		<option value="-">-</option>
		<option value="?page=home"<?php echo (!isset($_GET["page"]) || $_GET["page"]=="" || $_GET["page"]=="home" ? " selected='selected'" : ""); ?>>HOME</option>
		<option value="?page=blog"<?php echo (isset($_GET["page"]) && ($_GET["page"]=="blog" || $_GET["page"]=="post") ? " selected='selected'" : ""); ?>>NEWS</option>
		<option value="?page=blog">- BLOG</option>
		<option value="?page=post">- SINGLE POST</option>
		<option value="?page=classes"<?php echo (isset($_GET["page"]) && $_GET["page"]=="classes" ? " selected='selected'" : ""); ?>>CLASSES</option>
		<option value="?page=classes#gym-fitness">- GYM FITNESS</option>
		<option value="?page=classes#indoor-cycling">- INDOOR CYCLING</option>
		<option value="?page=classes#yoga-pilates">- YOGA PILATES</option>
		<option value="?page=classes#cardio-fitness">- CARDIO FITNESS</option>
		<option value="?page=classes#boxing">- BOXING</option>
		<option value="?page=timetable"<?php echo (isset($_GET["page"]) && $_GET["page"]=="timetable" ? " selected='selected'" : ""); ?>>TIMETABLE</option>
		<option value="?page=timetable#gym-fitness">- GYM FITNESS</option>
		<option value="?page=timetable#indoor-cycling">- INDOOR CYCLING</option>
		<option value="?page=timetable#yoga-pilates">- YOGA PILATES</option>
		<option value="?page=timetable#cardio-fitness">- CARDIO FITNESS</option>
		<option value="?page=timetable#boxing">- BOXING</option>
		<option value="?page=gallery"<?php echo (isset($_GET["page"]) && $_GET["page"]=="gallery" ? " selected='selected'" : ""); ?>>GALLERY</option>
		<option value="?page=gallery#filter=.gym-fitness">- GYM FITNESS</option>
		<option value="?page=gallery#filter=.indoor-cycling">- INDOOR CYCLING</option>
		<option value="?page=gallery#filter=.yoga-pilates">- YOGA PILATES</option>
		<option value="?page=gallery#filter=.cardio-fitness">- CARDIO FITNESS</option>
		<option value="?page=gallery#filter=.boxing">- BOXING</option>
		<option value="?page=contact"<?php echo (isset($_GET["page"]) && $_GET["page"]=="contact" ? " selected='selected'" : ""); ?>>CONTACT</option>
	</select>
</div>