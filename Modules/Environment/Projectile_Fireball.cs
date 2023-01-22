datablock ParticleData(EOTWFireballParticle)
{
	dragCoefficient = 1;
	windCoefficient = 1;
	gravityCoefficient = -0.4;
	inheritedVelFactor = 0.5;
	constantAcceleration = 0;
	lifetimeMS = 2000;
	lifetimeVarianceMS = 500;
	spinSpeed = 0;
	spinRandomMin = -900;
	spinRandomMax = 900;
	useInvAlpha = false;
	framesPerSec = 1;
	textureName = "base/data/particles/cloud";

	colors[0] = "1.0 0.0 0.0 1.0";
	colors[1] = "1.0 0.5 0.0 1.0";
	colors[2] = "1.0 0.0 0.0 1.0";
	colors[3] = "0.0 0.0 0.0 1.0";

	sizes[0] = 2;
	sizes[1] = 2.5;
	sizes[2] = 2;
	sizes[3] = 1;

	times[0] = 0;
	times[1] = 0.3;
	times[2] = 0.6;
	times[3] = 1;
};
datablock ParticleEmitterData(EOTWFireballEmitter)
{
	ejectionPeriodMS = 20;
	periodVarianceMS = 5;
	ejectionVelocity = 2;
	velocityVariance = 1;
	ejectionOffset = 0;
	thetaMin = 45;
	thetaMax = 135;
	phiReferenceVel = 0;
	phiVariance = 360;
	particles = EOTWFireballParticle;
	lifetimeMS = 0;
	lifetimeVarianceMS = 0;
	uiName = "EOTW Meteor Emitter";
};

datablock ParticleData(EOTWFireballExplosionParticle)
{
	dragCoefficient = 1;
	gravityCoefficient = -0.4;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0;
	lifetimeMS = 650;
	lifetimeVarianceMS = 350;
	textureName = "base/data/particles/cloud";
	spinSpeed = 10;
	spinRandomMin = -50;
	spinRandomMax = 50;
	
	colors[0] = "1.0 0.0 0.0 1.0";
	colors[1] = "1.0 0.5 0.0 1.0";
	colors[2] = "1.0 0.0 0.0 1.0";
	colors[3] = "0.0 0.0 0.0 1.0";

	sizes[0] = 2;
	sizes[1] = 2.5;
	sizes[2] = 2;
	sizes[3] = 1;

	times[0] = 0;
	times[1] = 0.3;
	times[2] = 0.6;
	times[3] = 1;

	useInvAlpha = false;
};

datablock ParticleEmitterData(EOTWFireballExplosionEmitter)
{
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 10;
	velocityVariance = 1.0;
	ejectionOffset = 0.5;
	thetaMin = 0;
	thetaMax = 80;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	particles = "EOTWFireballExplosionParticle";
	uiName = "EOTW Meteor Explosion";
};

datablock ExplosionData(EOTWFireballExplosion)
{
	lifeTimeMS = 500;
	particleEmitter = EOTWFireballExplosionEmitter;
	particleDensity = 25;
	particleRadius = 0.5;
	faceViewer = true;
	explosionScale = "1 1 1";
	shakeCamera = false;
	camShakeFreq = "1 1 1";
	camShakeAmp = "1 1 1";
	camShakeDuration = 0;
	camShakeRadius = 0;
	lightStartRadius = 15;
	lightEndRadius = 0;
	lightStartColor = "1 0 0";
	lightEndColor = "1 0 0";
	damageRadius = 3;
	radiusDamage = 100;
	impulseRadius = 6;
	impulseForce = 4000;
};

datablock ProjectileData(EOTWFireballProjectile)
{
	shapeFile = "base/data/shapes/empty.dts";
	directDamage = 1337;
	radiusDamageType = $DamageType::EOTWFireball;
	brickExplosionRadius = 0;
	brickExplosionImpact = 0;
	impactImpulse = 0;
	verticalImpulse = 0;
	explosion = EOTWFireballExplosion;
	particleEmitter = EOTWFireballEmitter;
//	sound = FireLoopSound;
	muzzleVelocity = 60;
	velInheritFactor = 1;
	armingDelay = 0;
	lifetime = 30000;
	fadeDelay = 3500;
	bounceElasticity = 0.5;
	bounceFriction = 0.20;
	isBallistic = 1;
	gravityMod = 0.1;
	hasLight = 1;
	lightRadius = 5;
	lightColor = "1.0 0.0 0.0";
	uiName = "";
};

function EOTWFireballProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	initContainerRadiusSearch(%pos, 3, $Typemasks::fxBrickAlwaysObjectType);
	while(isObject(%hit = containerSearchNext()))
	{
		if(isObject(%matter = getMatterType(%hit.Material)) && !%matter.meteorImmune)
		{
			if(!isObject(containerRaycast(%hit.getPosition(), %pos, $Typemasks::fxBrickObjectType | $Typemasks::StaticShapeObjectType, %hit)))
			{
				%hit.dontRefund = true;
				%hit.killBrick();
			}
		}
	}			
}