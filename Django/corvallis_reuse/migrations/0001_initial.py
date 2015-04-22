# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations
import uuid


class Migration(migrations.Migration):

    dependencies = [
    ]

    operations = [
        migrations.CreateModel(
            name='category',
            fields=[
                ('cat_id', models.UUIDField(default=uuid.uuid4, serialize=False, editable=False, primary_key=True)),
                ('cat_name', models.CharField(max_length=50)),
            ],
        ),
        migrations.CreateModel(
            name='item',
            fields=[
                ('item_id', models.UUIDField(default=uuid.uuid4, serialize=False, editable=False, primary_key=True)),
                ('item_name', models.CharField(max_length=50)),
            ],
        ),
        migrations.CreateModel(
            name='item_category',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('cat', models.ForeignKey(to='corvallis_reuse.category')),
                ('item', models.ForeignKey(to='corvallis_reuse.item')),
            ],
        ),
        migrations.CreateModel(
            name='organization',
            fields=[
                ('org_id', models.UUIDField(default=uuid.uuid4, serialize=False, editable=False, primary_key=True)),
                ('org_name', models.CharField(max_length=50)),
                ('org_phone', models.CharField(max_length=15, null=b'True')),
                ('org_address1', models.CharField(max_length=60, null=b'True')),
                ('org_address2', models.CharField(max_length=60, null=b'True')),
                ('org_address3', models.CharField(max_length=60, null=b'True')),
                ('org_zip', models.CharField(max_length=10, null=b'True')),
                ('org_site', models.URLField(null=b'True')),
                ('org_notes', models.CharField(max_length=200, null=b'True')),
            ],
        ),
        migrations.CreateModel(
            name='repairable',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('item', models.ForeignKey(to='corvallis_reuse.item')),
                ('org', models.ForeignKey(to='corvallis_reuse.organization')),
            ],
        ),
        migrations.CreateModel(
            name='reusable',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('item', models.ForeignKey(to='corvallis_reuse.item')),
                ('org', models.ForeignKey(to='corvallis_reuse.organization')),
            ],
        ),
    ]
