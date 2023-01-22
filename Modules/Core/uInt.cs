// ================
// Unsigned Integer safe arithemtic operators
// ================
// Author: McTwist
// Description: Safely operates unsigned integer numbers together
// License: Free to use
// ================

// Safely add two values together like they are unsigned
// Algorithm: https://forum.blockland.us/index.php?topic=248922.msg7177640#msg7177640
function uint_add(%a, %b)
{
	return ((%a | 0) + (%b | 0)) | 0;
	// Algorithm: https://github.com/rubycon/isaac.js/blob/master/isaac.js#L106
	//%lsb = (%a & 0xffff) + (%b & 0xffff);
	//%msb = (%a >> 16) + (%b >> 16) + (%lsb >> 16);
	//return (%msb << 16) | (%lsb & 0xffff);
}

// Safely subtract two values together like they are unsigned
function uint_sub(%a, %b)
{
	if (%b == 0)
		return %a;
	return uint_add(%a, uint_add(~%b, 1));
	// Algorithm: https://www.geeksforgeeks.org/subtract-two-numbers-without-using-arithmetic-operators/
	//return uint_sub(%a ^ %b, (~%a & %b) << 1);
}

// Safely multiplicate two values together like they are unsigned
// Algorithm: https://stackoverflow.com/a/14663667
function uint_mul(%a, %b)
{
	%reg = 0;

	while (%b != 0)
	{
		if (%b & 1)
			%reg = uint_add(%reg, %a);
		%a <<= 1;
		%b >>= 1;
	}

	return %reg;
}

// Unsigned division with remainder
// Use $uint::remainder for modulus result
// Algorithm: http://www.bearcave.com/software/divide.htm
function uint_div(%a, %b)
{
	%q = 0;
	%r = 0;

	if (%b == 0)
	{
		$uint::remainder = 0;
		return 0;
	}
	if (%a == %b)
	{
		$uint::remainder = 0;
		return 1;
	}
	if (%a < %b)
	{
		$uint::remainder = %a;
		return 0;
	}

	%n = 32;

	while (%r < %b)
	{
		%bit = (%a & 0x80000000) >> 31;
		%r = (%r << 1) | %bit;
		%d = %a;
		%a <<= 1;
		%n--;
	}

	%a = %d;
	%r >>= 1;
	%n++;

	for (%i = 0; %i < %n; %i++) {
		%bit = (%a & 0x80000000) >> 31;
		%r = (%r << 1) | %bit;
		%t = uint_sub(%r - %b);
		%qq = !((%t & 0x80000000) >> 31);
		%a <<= 1;
		%q = (%q << 1) | %qq;
		if (%qq) {
			%r = %t;
		}
	}

	$uint::remainder = %r;
	return %q;
}

// Unsigned division, ceiling the value depending on remainder
function uint_divCeil(%a, %b)
{
	%q = uint_div(%a, %b);
	if ($uint::remainder > 0)
		%q = uint_add(%q, 1);
	$uint::remainder = 0;
	return %q;
}

// Safely apply power of two unsigned values
// Algorithm: https://stackoverflow.com/a/101613
function uint_pow(%a, %b)
{
	%r = 1;
	while (%b != 0)
	{
		if (%b & 1)
			%r = uint_mul(%r, %a);
		%b >>= 1;
		%a = uint_mul(%a, %a);
	}

	return %r;
}