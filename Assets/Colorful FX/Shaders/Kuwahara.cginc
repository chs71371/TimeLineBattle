// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

#ifndef RADIUS
#define RADIUS 3
#endif

#define H3Z half3(0.0, 0.0, 0.0)

sampler2D _MainTex;
half2 _PSize;

half4 frag(v2f_img i) : SV_Target
{
	half3 m0 = H3Z; half3 m1 = H3Z; half3 m2 = H3Z; half3 m3 = H3Z;
	half3 s0 = H3Z; half3 s1 = H3Z; half3 s2 = H3Z; half3 s3 = H3Z;
	int k, j;
	
	// Loop 1
	for (j = -RADIUS; j <= 0; j++)
	{
		for (k = -RADIUS; k <= 0; k++)
		{
			half3 c = tex2D(_MainTex, i.uv + half2(k, j) * _PSize).rgb;
			m0 += c;
			s0 += c * c;
		}
	}
					
	// Loop 2
	for (j = -RADIUS; j <= 0; j++)
	{
		for (k = 0; k <= RADIUS; k++)
		{
			half3 c = tex2D(_MainTex, i.uv + half2(k, j) * _PSize).rgb;
			m1 += c;
			s1 += c * c;
		}
	}
					
	// Loop 3
	for (j = 0; j <= RADIUS; j++)
	{
		for (k = 0; k <= RADIUS; k++)
		{
			half3 c = tex2D(_MainTex, i.uv + half2(k, j) * _PSize).rgb;
			m2 += c;
			s2 += c * c;
		}
	}
					
	// Loop 4
	for (j = 0; j <= RADIUS; j++)
	{
		for (k = -RADIUS; k <= 0; k++)
		{
			half3 c = tex2D(_MainTex, i.uv + half2(k, j) * _PSize).rgb;
			m3 += c;
			s3 += c * c;
		}
	}

	const half n = half((RADIUS + 1) * (RADIUS + 1));
	half minSigma2 = 1e+2;
	half3 color = H3Z;

	// Loop 1
	m0 /= n;
	s0 = abs(s0 / n - m0 * m0);
	half sigma2 = s0.r + s0.g + s0.b;
	if (sigma2 < minSigma2) { minSigma2 = sigma2; color = m0; }

	// Loop 2
	m1 /= n;
	s1 = abs(s1 / n - m1 * m1);
	sigma2 = s1.r + s1.g + s1.b;
	if (sigma2 < minSigma2) { minSigma2 = sigma2; color = m1; }

	// Loop 3
	m2 /= n;
	s2 = abs(s2 / n - m2 * m2);
	sigma2 = s2.r + s2.g + s2.b;
	if (sigma2 < minSigma2) { minSigma2 = sigma2; color = m2; }

	// Loop 4
	m3 /= n;
	s3 = abs(s3 / n - m3 * m3);
	sigma2 = s3.r + s3.g + s3.b;
	if (sigma2 < minSigma2) { minSigma2 = sigma2; color = m3; }

	return half4(color, 1.0);
}
