// Hack to get and set arbitrary global variables
// Much cleaner than implementing this in DLL
function get_var(%name) {
	%first = strLwr(getSubStr(%name, 0, 1));
	%rest = getSubStr(%name, 1, strLen(%name));
	switch$(%first) {
		case "a": return $a[%rest];
		case "b": return $b[%rest];
		case "c": return $c[%rest];
		case "d": return $d[%rest];
		case "e": return $e[%rest];
		case "f": return $f[%rest];
		case "g": return $g[%rest];
		case "h": return $h[%rest];
		case "i": return $i[%rest];
		case "j": return $j[%rest];
		case "k": return $k[%rest];
		case "l": return $l[%rest];
		case "m": return $m[%rest];
		case "n": return $n[%rest];
		case "o": return $o[%rest];
		case "p": return $p[%rest];
		case "q": return $q[%rest];
		case "r": return $r[%rest];
		case "s": return $s[%rest];
		case "t": return $t[%rest];
		case "u": return $u[%rest];
		case "v": return $v[%rest];
		case "w": return $w[%rest];
		case "x": return $x[%rest];
		case "y": return $y[%rest];
		case "z": return $z[%rest];
		case "_": return $_[%rest];
	}
	//error("get_var: invalid variable name " @ %name);
	return "";
}
function set_var(%name, %val) {
	%first = strLwr(getSubStr(%name, 0, 1));
	%rest = getSubStr(%name, 1, strLen(%name));
	switch$(%first) {
		case "a": $a[%rest] = %val; return;
		case "b": $b[%rest] = %val; return;
		case "c": $c[%rest] = %val; return;
		case "d": $d[%rest] = %val; return;
		case "e": $e[%rest] = %val; return;
		case "f": $f[%rest] = %val; return;
		case "g": $g[%rest] = %val; return;
		case "h": $h[%rest] = %val; return;
		case "i": $i[%rest] = %val; return;
		case "j": $j[%rest] = %val; return;
		case "k": $k[%rest] = %val; return;
		case "l": $l[%rest] = %val; return;
		case "m": $m[%rest] = %val; return;
		case "n": $n[%rest] = %val; return;
		case "o": $o[%rest] = %val; return;
		case "p": $p[%rest] = %val; return;
		case "q": $q[%rest] = %val; return;
		case "r": $r[%rest] = %val; return;
		case "s": $s[%rest] = %val; return;
		case "t": $t[%rest] = %val; return;
		case "u": $u[%rest] = %val; return;
		case "v": $v[%rest] = %val; return;
		case "w": $w[%rest] = %val; return;
		case "x": $x[%rest] = %val; return;
		case "y": $y[%rest] = %val; return;
		case "z": $z[%rest] = %val; return;
		case "_": $_[%rest] = %val; return;
	}
	//error("set_var: invalid variable name " @ %name);
	return "";
}
function get_var_obj(%obj, %name) {
	if(!isObject(%obj)) {
		//error("set_var_obj: object " @ %obj @ " does not exist");
		return;
	}
	%first = strLwr(getSubStr(%name, 0, 1));
	%rest = getSubStr(%name, 1, strLen(%name));
	switch$(%first) {
		case "a": return %obj.a[%rest];
		case "b": return %obj.b[%rest];
		case "c": return %obj.c[%rest];
		case "d": return %obj.d[%rest];
		case "e": return %obj.e[%rest];
		case "f": return %obj.f[%rest];
		case "g": return %obj.g[%rest];
		case "h": return %obj.h[%rest];
		case "i": return %obj.i[%rest];
		case "j": return %obj.j[%rest];
		case "k": return %obj.k[%rest];
		case "l": return %obj.l[%rest];
		case "m": return %obj.m[%rest];
		case "n": return %obj.n[%rest];
		case "o": return %obj.o[%rest];
		case "p": return %obj.p[%rest];
		case "q": return %obj.q[%rest];
		case "r": return %obj.r[%rest];
		case "s": return %obj.s[%rest];
		case "t": return %obj.t[%rest];
		case "u": return %obj.u[%rest];
		case "v": return %obj.v[%rest];
		case "w": return %obj.w[%rest];
		case "x": return %obj.x[%rest];
		case "y": return %obj.y[%rest];
		case "z": return %obj.z[%rest];
		case "_": return %obj._[%rest];
	}
	//error("get_var_obj: invalid variable name " @ %name);
	return "";
}
function set_var_obj(%obj, %name, %val) {
	if(!isObject(%obj)) {
		//error("set_var_obj: object " @ %obj @ " does not exist");
		return;
	}
	%first = strLwr(getSubStr(%name, 0, 1));
	%rest = getSubStr(%name, 1, strLen(%name));
	switch$(%first) {
		case "a": %obj.a[%rest] = %val; return;
		case "b": %obj.b[%rest] = %val; return;
		case "c": %obj.c[%rest] = %val; return;
		case "d": %obj.d[%rest] = %val; return;
		case "e": %obj.e[%rest] = %val; return;
		case "f": %obj.f[%rest] = %val; return;
		case "g": %obj.g[%rest] = %val; return;
		case "h": %obj.h[%rest] = %val; return;
		case "i": %obj.i[%rest] = %val; return;
		case "j": %obj.j[%rest] = %val; return;
		case "k": %obj.k[%rest] = %val; return;
		case "l": %obj.l[%rest] = %val; return;
		case "m": %obj.m[%rest] = %val; return;
		case "n": %obj.n[%rest] = %val; return;
		case "o": %obj.o[%rest] = %val; return;
		case "p": %obj.p[%rest] = %val; return;
		case "q": %obj.q[%rest] = %val; return;
		case "r": %obj.r[%rest] = %val; return;
		case "s": %obj.s[%rest] = %val; return;
		case "t": %obj.t[%rest] = %val; return;
		case "u": %obj.u[%rest] = %val; return;
		case "v": %obj.v[%rest] = %val; return;
		case "w": %obj.w[%rest] = %val; return;
		case "x": %obj.x[%rest] = %val; return;
		case "y": %obj.y[%rest] = %val; return;
		case "z": %obj.z[%rest] = %val; return;
		case "_": %obj._[%rest] = %val; return;
	}
	//error("set_var_obj: invalid variable name " @ %name);
	return "";
}