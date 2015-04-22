import uuid
from django.db import models

# Create your models here.
class organization(models.Model):
    org_id = models.UUIDField(primary_key=True, default=uuid.uuid4, editable=False)
    org_name = models.CharField(max_length=50)
    org_phone = models.CharField(max_length=15, null='True')
    org_address1 = models.CharField(max_length=60, null='True')
    org_address2 = models.CharField(max_length=60, null='True')
    org_address3 = models.CharField(max_length=60, null='True')
    org_zip = models.CharField(max_length=10, null='True')
    org_site = models.URLField(max_length=200, null='True')
    org_notes = models.CharField(max_length=200, null='True')

class item(models.Model):
    item_id = models.UUIDField(primary_key=True, default=uuid.uuid4, editable=False)
    item_name = models.CharField(max_length=50)

class repairable(models.Model):
    item = models.ForeignKey(item)
    org = models.ForeignKey(organization)

class reusable(models.Model):
    item = models.ForeignKey(item)
    org = models.ForeignKey(organization)

class category(models.Model):
    cat_id = models.UUIDField(primary_key=True, default=uuid.uuid4, editable=False)
    cat_name = models.CharField(max_length=50)

class item_category(models.Model):
    item = models.ForeignKey(item)
    cat = models.ForeignKey(category)